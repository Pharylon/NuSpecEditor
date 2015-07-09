# NuSpecEditor
A simple utility for updating NuSpec files. Invoke from the command line to change version, or release notes for any nuspec files.

To use:
---------------
1. Put the .exe somewhere and like C:\utils and then make sure that directory is included in the PATH environment variable.
2. On the command line, go to the directory your nuspec file is in and invoke it.

Arguments are -r for recursively hitting all nuspec files in directory, -v for updating version, -n for updating notes. Example:

    C:\Projects\Myproj> NuspecEditor -v 2.8 -n "Some change notes" -r