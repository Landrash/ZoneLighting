﻿using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ZoneLighting.ZoneProgramNS;

namespace WebRemoteNew.API
{
    [Route("api/[controller]")]
    public class ZLMController : Controller
    {
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/values
        [HttpGet]
        public IEnumerable<ProgramSet> Get()
        {
            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        public string Get(string command)
        {
            if (command == "initialize")
            {
                ZLM.I.Initialize(false, RunnerHelpers.AddBasementZonesAndProgramsWithSync());
                return "Success";
            }
            if (command == "uninitialize")
            {
                ZLM.I.Uninitialize();
                return "Success";
            }

            return "Failure";

            //if (command == "initialize")
            //{
            //	ZLM.I.Initialize(loadExternalZones: false);
            //	return "Success";
            //}
            //if (command == "initialize")
            //{
            //	ZLM.I.Initialize(loadExternalZones: false);
            //	return "Success";
            //}
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}