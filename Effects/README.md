# Effects

Each `Effect.fx` file is simply a ZIP file containing a number of PNG images that make up the animation sequence. Each file is numbered from 0 to X depending on how many frames are in the animation. 
SteamAPNG takes these images and stitches them together to make the animation. 

## Making your Own

### Adobe After Effects

1. Create your effect, making sure that toggling transparency works (it should show a checkered grid on your composition background) 
2. Export as PNG sequence making sure to choose `RGB + Alpha` for the Video Output. 
    - See [here](https://www.premiumbeat.com/blog/quick-tip-exporting-with-alpha-channels-in-after-effects/) for detailed instructions
3. Add all the outputted images to a new ZIP file. Rename the file from `XXXXX.zip` to `XXXXX.fx`
4. Drop the fx file into the `Effects` Folder.
5. Make a pull request on GitHub to share the effect here. 
