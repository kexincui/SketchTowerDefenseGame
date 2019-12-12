# Sketch-based Tower Defense Game
In the CSCE624 final project, we built a game combined with sketch recognition. Our purpose is helping users to get interest in drawing sketches. We use Unity as our auxiliary software. In this game, we collect the input drawing data from users and use $P algorithm to match our predefined templates.

# Requirement
We use Unity 2018.4.12f1 as our platform.

# Test directly on Unity
Open the project in the folder contained all the files in this github. Then, select and open the StartMenu Scence in the Assest\Scences. You can click Play button to run the program.

# Build and Run on PC.
Click build setting under File menu. Select the platform, add open scenes (StartMenu and FirstScene) and build the code. Before you run the game, you should copy the Assest/Data to (Mac player: "path to player app bundle/Contents", Win/Linux player: "path to executablename_Data folder"). Finally, you can run and play the game.
  
# Build and Run on IOS.
Click build setting under File menu. Select the platform, add open scenes (StartMenu and FirstScene) and build the code. Then, you should copy the the Assest/Data to "path to player app bundle/AppName.app/Data" (The previous Data folder should be under the latter folder Data). Then, open the folder "path to player app bundle/AppName.app", run the xcodeproj with XCode, log in with an Apple Acount, change Bunder Identifier and turn automatic signing on. After that, you can connect to you iOS device and build the game on your device. After building, you can, finally, run the game .
