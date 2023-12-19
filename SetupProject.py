from os.path import join, exists
from sys import exit

# Reads the steam library VDF file to find where Lethal Company is installed
if not exists("C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf"):
    libraryFolderDb = input("libraryfolders.vdf was not found at: C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf\nEnter 'D' for: D:\\SteamLibrary\\libaryfolder.vdf\n\nOr enter the full path to it (\\\\ seperators!):\n")
    if(libraryFolderDb == "D"): libraryFolderDb = "D:\\SteamLibrary\\libraryfolder.vdf"
    if not exists(libraryFolderDb):
        input(f'{libraryFolderDb}\nWas not found as an existing .vdf file on your system.\nPress any key to close.')
        exit(1)
else:
    libraryFolderDb = "C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf"
    print("vdf found at: "+libraryFolderDb)

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
            lc_path = join(steamAppDirectory, "steamapps\\common\\Lethal Company") # Lethal Company Path

# Ensures path exists before modifying project file
if (lc_path == ""):
    print("Unable to find Lethal Company path.")
    lc_path = input("Enter path to Lethal Company (Remember to format with \\\\ as seperators):\n")
    if not exists(lc_path):
        input(f'{lc_path}\nWas not found as an existing directory on your system.\nPress any key to close.')
        exit(1)

# Replaces referenced assembly paths in the project file to match computer configuration
replace_paths = {}

# Lethal Company Managed Assembly Path
replace_paths["{{LC_PATH}}"] = join(lc_path, "Lethal Company_Data\\Managed")

# BepInEx Assembly Path
replace_paths["{{BEPINEX_ASSEMBLY_PATH}}"] = join(lc_path, "BepInEx\\core")

# BepInEx Plugin Path
replace_paths["{{BEPINEX_PLUGIN_PATH}}"] = join(lc_path, "BepInEx\\plugins")

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
input("\n\nSUCCESS\n\nPress Enter to close")
