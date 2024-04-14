# Bug Reporting
<details>
<summary> Steps before creating a bug report </summary>
<br>

- Check the [issues](https://github.com/Malcolm-Q/LC-LateGameUpgrades/issues) page of the repository and check if there's any already reported issue. If there is one, use that one instead of making a new one.
- Ensure that the issue you are reporting is not an intended feature of this mod.

</details>

<details>
<summary> Steps when creating a bug report </summary>
<br>

- Locate the ``LogOutput.log`` file in the ``BepInEx`` folder and post it in the bug report.
   - This file will always contain the logs of the **last** game instance.
   - It will help me find where the root of the issue might be.
- Post the list of mods (or code relevant to the modpack) if they are relevant to the issue
   - E.g a feature works without a mod, stops working when that mod is present. Also applies to the other mod's features.
   - If unsure if a certain feature is affected by some other mod, post the list regardless. Better to have more than less.
- Try to be as descriptive as possible when creating the bug report.
    - Saying only that something isn't working might not always be sufficient. I might need more details about the state of the game or the configurations used when the issue occurs.

<details>
<summary> Optional Steps </summary>
<br>

- List of steps to reproduce the issue if the issue is consistent
   - Having a list of steps can help further figuring the cause of the issue to resolve it.
   - This won't always be a case for inconsistent issues so try to be as descriptive when it happens

- Suggestion to resolve the issue reported
   - This would require coding / game code knowledge
   - The suggestion has to be reasonable and objectively better or equivalent than the current implementation
   - You can make [pull requests](https://github.com/Malcolm-Q/LC-LateGameUpgrades/pulls) for these suggestions rather than make an issue and I will be able to review it and possibly accept it if it's all good.

</details>

</details>