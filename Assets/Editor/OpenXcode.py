#!/usr/bin/python
## -*- coding: utf-8 -*-
import subprocess
import sys

def open_xcode_project():
    try:
        print "sys.path[0]:",sys.path[0]
        print "scriptPath:", sys.argv[0]
        xcodeProjectPath = sys.argv[1]
        subprocess.call(["open", "/Applications/Xcode.app",xcodeProjectPath])
        print "completed open xcode"
        return 0
        raise FileExistsError("Could not open xcode!")
    except Exception as e:
      print "ERROR open xcode, error: ", e
      return 1

sys.exit(open_xcode_project())
