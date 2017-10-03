git clone %1 temp
rmdir /s /q "temp/.git"
robocopy temp . *.* /e /is
rmdir /s /q temp