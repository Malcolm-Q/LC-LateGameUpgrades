import os
import sys

# Reads the steam library VDF file to find where Lethal Company is installed
libraryFolderDb = "C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf"

vs_project_file = ".\\MoreShipUpgrades\\MoreShipUpgrades.csproj"

lc_path = ""

# Parsing through steam library vdf
with open(libraryFolderDb, "r") as file:
    steamAppDirectory = ""

    for line in file:
        line = line.strip()
        if (line.startswith('"path"')): # Parsing (messily) through the steam library folders vdf
            steamAppDirectory = line.split('"path"')[1].strip().replace("\\\\", "\\")[1:-1] # Steam data directory
            #print(steamAppDirectory)

        elif (line.startswith('"1966720"')): # Checks for Lethal Company app id
            lc_path = os.path.join(steamAppDirectory, "steamapps\\common\\Lethal Company") # Lethal Company Path

# Ensures path exists before modifying project file
if (lc_path == ""):
    print("Unable to find Lethal Company path.")
    sys.exit(1)

# Replaces referenced assembly paths in the project file to match computer configuration
replace_paths = {}

# Lethal Company Managed Assembly Path
replace_paths["{{LC_PATH}}"] = os.path.join(lc_path, "Lethal Company_Data\\Managed")

# BepInEx Assembly Path
replace_paths["{{BEPINEX_ASSEMBLY_PATH}}"] = os.path.join(lc_path, "BepInEx\\core")

# BepInEx Plugin Path
replace_paths["{{BEPINEX_PLUGIN_PATH}}"] = os.path.join(lc_path, "BepInEx\\plugins")

# Replace reference string with corrected paths
lines = []
with open(vs_project_file, "r") as file:
    lines = file.readlines()

# Does the replacement
for i in range(len(lines)):
    for rep in replace_paths.keys():
        lines[i] = lines[i].replace(rep, replace_paths[rep])

# Writes changes
with open(vs_project_file, "w") as file:
    file.writelines(lines)

# Finalize Output
for key in replace_paths.keys():
    print(key + " -> " + replace_paths[key])