# Get SDF data from Texture3D
This script is able to get real SDF data from Texture 3D in Unity3D,and serilize it into a Json file.


How to use:


1.Download all files(Jsonserilization.cs and SDF2Json.cs)

2.Drug these two scripts into your Unity project.

3.Add SDF2Json.cs to a Gameobeject.

4.Choose the target Texture 3D,and input the nesseary data(Box center and Desired Box size.You can set these value when you bake SDF)

5.Press the play button, and you can find the Json-path in console.

Note:


1.I have eliminate the coordinate offset in the script, thus the SDF data is based on the global coordinate system.

2.This script generate normalized SDF data, which means if you want to get the real SDF data you have to mutiply your data by Maxresolution and Gridsize.(you can find Maxresolution and Gridsize in Json file.)
