using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using Anshul.Utilities;
using LightingControllerBase;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using ZoneLighting.MEF;
using ZoneLighting.ZoneNS;

namespace ZoneLighting.ZoneProgramNS.Factories
{
    /// <summary>
    /// Responsible for managing heavy tasks related to zones, especially those that 
    /// need interaction with external modules such as creating programs in zones. Part of 
    /// that responsibility also inclues interacting with MEF to load the external programs.
    /// </summary>
	public class ZoneScaffolder
    {
        #region Singleton

        private static ZoneScaffolder _instance;
        public static ZoneScaffolder Instance => _instance ?? (_instance = new ZoneScaffolder());

        #endregion

        #region CORE

        public BetterList<ILightingController> LightingControllers { get; } =
            new BetterList<ILightingController>();

        #endregion

        #region MEF

        [ImportMany(typeof(ZoneProgram), AllowRecomposition = true)]
        public IList<ExportFactory<ZoneProgram, IZoneProgramMetadata>> ZoneProgramFactories { get; set; }

        [ImportMany(typeof(ILightingController), AllowRecomposition = true)]
        public IList<ExportFactory<ILightingController, ILightingControllerMetadata>> LightingControllerFactories { get; set; }

        public List<LightingControllerInfo> LightingControllerInfos { get; set; } = new List<LightingControllerInfo>();

        public class LightingControllerInfo
        {
            public string ConfigString { get; set; }

            public dynamic Config { get; set; }

            public ExportFactory<ILightingController, ILightingControllerMetadata> Factory { get; set; }
        }

        /// <summary>
        /// Container for the external modules.
        /// </summary>
        private CompositionContainer ModuleContainer { get; set; }

        #endregion

        #region C+I

        public bool Initialized { get; private set; }

        public void Initialize(string programModuleDirectory, string lightingControllerModuleDirectory)
        {
            if (!Initialized)
            {
                LightingControllerFactories = new List<ExportFactory<ILightingController, ILightingControllerMetadata>>();
                ZoneProgramFactories = new List<ExportFactory<ZoneProgram, IZoneProgramMetadata>>();
                LoadModules(programModuleDirectory, lightingControllerModuleDirectory);
                Initialized = true;
            }
        }

        public void InitLightingControllers()
        {
            //for now - initialize using infos loaded based on json
            //later - this needs to be driven by the user somehow - maybe during setup of ZL
            //or manually when they wanna add new controllers
            LightingControllerInfos.ForEach(info =>
            {
                var factory = info.Factory;
                var lightingController = factory.CreateExport().Value;
                lightingController.Initialize(info.ConfigString);
                LightingControllers.Add(lightingController);
            });
        }

        public void UninitLightingControllers()
        {
            LightingControllers.ForEach(x => x.Uninitialize());
        }

        public void Uninitialize()
        {
            if (Initialized)
            {
                UninitLightingControllers();
                ModuleContainer?.Dispose();
                ModuleContainer = null;
                ZoneProgramFactories = null;
                Initialized = false;
            }
        }

        /// <summary>
        /// Loads external ZoneProgram modules.
        /// </summary>
        private void LoadModules(string programModuleDirectory = null, string lightingControllerModuleDirectory = null)
        {
            var fileCatalogs = new List<ComposablePartCatalog>();

            //need to set this because otherwise for web projects, the file is loaded from c:\windows\system32\inetsrv probably
            //because that's where w3wp.exe is or something.. who knows.
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrEmpty(programModuleDirectory))
                LoadProgramModules(programModuleDirectory, fileCatalogs);
            if (!string.IsNullOrEmpty(lightingControllerModuleDirectory))
                LoadLightingControllerModules(lightingControllerModuleDirectory, fileCatalogs);

            var aggregateCatalog = new AggregateCatalog(fileCatalogs);
            ModuleContainer = new CompositionContainer(aggregateCatalog);
            ModuleContainer.ComposeParts(this);

            //lighting controller infos are loaded after everything 
            //because they require the ComposeParts call to finish
            if (!string.IsNullOrEmpty(lightingControllerModuleDirectory))
                LoadLightingControllerInfos(lightingControllerModuleDirectory);
        }

        private void LoadLightingControllerModules(string lightingControllerModuleDirectory, List<ComposablePartCatalog> fileCatalogs)
        {
            foreach (var file in Directory.GetFiles(lightingControllerModuleDirectory, "*.dll").ToList())
            {
                var assembly = Assembly.LoadFrom(file);

                if (assembly.GetCustomAttributesData()
                    .Any(ass => ass.AttributeType.FullName == typeof(LightingControllerAssemblyAttribute).FullName))
                {
                    fileCatalogs.Add(new AssemblyCatalog(assembly));

                    foreach (var referencedFile in Directory.GetFiles(Path.GetDirectoryName(file), "*.dll").ToList())
                    {
                        //TODO: this try-catch needs to be replaced with a function that actually checks to see the file before copying
                        //TODO: can this whole process be removed completely? this seesm hacky because
                        //TODO: we are doing file copy to load dynamic modules, but ideally we should point
                        //TODO: to the location of the file instead of file copy? seems like a better way
                        try
                        {
                            File.Copy(referencedFile,
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(referencedFile)),
                                false);
                        }
                        catch (Exception ex)
                        {
                            // only copy if file is not in use
                        }
                    }
                }
            }


        }

        private void LoadLightingControllerInfos(string lightingControllerModuleDirectory)
        {
            if (!Directory.Exists(lightingControllerModuleDirectory))
                throw new Exception("Lighting Controller module directory does not exist.");

            foreach (var file in Directory.GetFiles(lightingControllerModuleDirectory, "*.json").ToList())
            {
                var configString = File.ReadAllText(file);
                dynamic config = JObject.Parse(configString);

                try
                {
                    var matchedLightingControllerFactories =
                        LightingControllerFactories.Where(lcf => lcf.Metadata.Name == config.Type.Value).ToList();

                    if (matchedLightingControllerFactories.Any())
                    {
                        if (matchedLightingControllerFactories.Count() <= 1)
                        {
                            var info = new LightingControllerInfo
                            {
                                ConfigString = configString,
                                Config = config,
                                Factory = matchedLightingControllerFactories.First()
                            };
                            LightingControllerInfos.Add(info);
                        }
                        else
                        {
                            throw new Exception("Too many matching Lighting Controllers.");
                        }
                    }
                }
                catch (RuntimeBinderException)
                {
                    Console.Write($"{file}: Could not read JSON or it is missing the type of LightingController.");
                }
            }

        }

        private void LoadProgramModules(string programModuleDirectory, List<ComposablePartCatalog> fileCatalogs)
        {
            if (!Directory.Exists(programModuleDirectory))
                throw new Exception("Programs module directory does not exist.");

            foreach (var file in Directory.GetFiles(programModuleDirectory, "*.dll").ToList())
            {
                var assembly = Assembly.LoadFrom(file);

                if (assembly.GetCustomAttributesData()
                    .Any(ass => ass.AttributeType == typeof(ZoneProgramAssemblyAttribute)))
                {
                    fileCatalogs.Add(new AssemblyCatalog(assembly));

                    foreach (var referencedFile in Directory.GetFiles(Path.GetDirectoryName(file), "*.dll").ToList())
                    {
                        //TODO: this try-catch needs to be replaced with a function that actually checks to see the file before copying
                        //TODO: can this whole process be removed completely? this seesm hacky because
                        //TODO: we are doing file copy to load dynamic modules, but ideally we should point
                        //TODO: to the location of the file instead of file copy? seems like a better way
                        try
                        {
                            File.Copy(referencedFile,
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(referencedFile)),
                                false);
                        }
                        catch (Exception ex)
                        {
                            // only copy if file is not in use
                        }
                    }
                }
            }
        }

        #endregion

        #region API

        public bool DoesProgramExist(string programName)
        {
            return GetAvailablePrograms().Contains(programName);
        }

        public IEnumerable<string> GetAvailablePrograms()
        {
            return ZoneProgramFactories.Select(x => x.Metadata.Name);
        }

        internal ZoneProgram CreateZoneProgram(string programName)
        {
            if (DoesProgramExist(programName))
                return ZoneProgramFactories.ToDictionary(x => x.Metadata.Name)[programName].CreateExport().Value;
            else
                throw new Exception($"No program by the name '{programName}' exists.");
        }

        /// <summary>
        /// Runs a zone with the given program name and starting values of the inputs as a name-value dictionary.
        /// </summary>
        public void RunZone(Zone zone, string programName, ISV isv = null, bool isSyncRequested = false, SyncContext syncContext = null, bool dontStart = false, dynamic startingParameters = null)
        {
            zone.Run(CreateZoneProgram(programName), isv, isSyncRequested, syncContext, dontStart, startingParameters);
        }


        public void AddInterruptingProgram(Zone zone, string programName, ISV isv = null, SyncContext syncContext = null)
        {
            var zoneProgram = CreateZoneProgram(programName);

            if (zoneProgram is ReactiveZoneProgram)
                zone.AddInterruptingProgram((ReactiveZoneProgram)zoneProgram, isv, syncContext);
            else
                throw new Exception("Given program is not a reactive program.");
        }

        /// <summary>
        /// Gets the names of all available programs.
        /// </summary>
        public IEnumerable<string> AvailableProgramNames
        {
            get { return ZoneProgramFactories.Select(x => x.Metadata.Name); }
        }

        //public Zone AddFadeCandyZone(BetterList<Zone> zones, string name, int numberOfLights, double? brightness = null)
        //{
        //    return AddZone(zones, name, FadeCandyController.Instance, numberOfLights, brightness);
        //}

        public Zone AddNodeMCUZone(BetterList<Zone> zones, string name, int numberOfLights, string lightingControllerName, double? brightness = null)
        {
            if (LightingControllers[lightingControllerName] == null)
            {
                Console.WriteLine($"No lighting controller with name {lightingControllerName} found.");
            }
            return AddZone(zones, name, LightingControllers[lightingControllerName], numberOfLights, brightness);
        }

        public Zone AddZone(BetterList<Zone> zones, string name, ILightingController lightingController, int numberOfLights,
            double? brightness = null)
        {
            //create new zone
            var zone = new Zone(lightingController, name, brightness);

            //add lights and add zone to collection
            zones.Add(zone).AddLights(numberOfLights);
            return zone;
        }


        #endregion

        #region Macro API




        public void CreateProgramSetsFromConfig()
        {

        }



        public void CreateProgramSetsFromConfig(string config)
        {

        }

        //      //TODO: Convert this method to CreateProgramSetsFromConfig or something like that. And another test called CreateZonesFromConfiguration.
        ///// <summary>
        ///// Initializes the given zones with information about the zone configuration saved in the zone configuration file.
        ///// Note that this method does not create any zones. It simply loads up the configuration and matches up the loaded configuration
        ///// with zones that already exist in zonesToLoadInto using the name. If there exist zones with the same name,
        ///// it will "map" the loaded zones to its respective complement in zonesToLoadInto.
        ///// </summary>
        ///// <param name="zonesToLoadInto">Zones to initialize new zones into</param>
        ///// <param name="configFile">If provided, use this configuration file to load zones from</param>
        //public bool InitializeFromZoneConfiguration(IList<Zone> zonesToLoadInto, string configFile)
        //{
        //	if (string.IsNullOrEmpty(configFile) || !File.Exists(configFile))
        //		return false;

        //	try
        //	{
        //		//strategy is to load a temporary list of zones from the configuration which will then be 
        //		//used to initialize zonesToLoadInto. Note that this temporary list of zones is scoped to this method
        //		//It is used only to get the important values like the program and input starting values and initialize
        //		//zonesToLoadInto from those values.
        //		var zonesToLoadFrom = ZoneConfig.DeserializeZones(File.ReadAllText(configFile));

        //		zonesToLoadFrom.ToList().ForEach(zoneToLoadFrom =>
        //		{
        //			if (zonesToLoadInto.Select(zone => zone.Name).Contains(zoneToLoadFrom.Name) && zoneToLoadFrom.ZoneProgram != null)
        //			{
        //				var zoneToLoadInto = zonesToLoadInto.First(z => zoneToLoadFrom.Name == z.Name);
        //				var zoneProgramName = zoneToLoadFrom.ZoneProgram.Name;
        //				ISV startingValues = zoneToLoadFrom.ZoneProgram.GetInputValues();

        //				//start the main program
        //				RunZone(zoneToLoadInto, zoneProgramName, startingValues);

        //				//TODO: start the interrupting programs
        //				//old shit
        //				//zoneToLoadFrom.InterruptingPrograms.ToList().ForEach(program =>
        //				//{
        //				//	zoneToLoadInto.AddInterruptingProgram(program, true, program.GetInputValues());
        //				//});
        //			}
        //		});
        //	}
        //	catch (Exception ex)
        //	{
        //		return false;
        //	}

        //	return true;
        //}

        #endregion


    }
}