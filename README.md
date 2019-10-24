# SteamAPNG
Generate APNG Images for Steam Game Covers.

## Prerequisites
- .NET Framework 4.7.2 (Included with Windows 10 1803+ by default)
- [FFMPEG](https://www.ffmpeg.org/) - Needs to be installed and [added to your PATH Environment Variable](https://www.thewindowsclub.com/how-to-install-ffmpeg-on-windows-10). 
## Installation
- Download the [latest release](https://github.com/IridiumIO/SteamAPNG/releases) of SteamAPNG
- Download the effects you want from [here](/Effects) and place them in an `Effects` folder next to the SteamAPNG.exe file.  
    *__Effects aren't included by default as the total download size would be ~400MB__*

## Usage
![](https://i.imgur.com/pykwynw.png)
1. Click on the Preview pane to select an image to apply an effect to. Images should ideally be `300x450` or `600x900` in size; other sizes will be scaled appropriately.
2. Choose the effect you want; the preview pane will update to show changes. 
3. Apply any transformation effects using the available sliders. Alternatively, mouse over the preview pane and use the following mouse/keyboard controls: 
    - `Mouse Scroll` Move the effect up and down
    - `Alt + Mouse Scroll` Move the effect left and right
    - `Ctrl + Mouse Scroll` Zoom the effect in and out
    - `Shift + Mouse Scroll` Rotate the effect
4. Choose output quality and size (See Output options below), select a filename and hit save.
    - Note: It could take up to 7 minutes to render depending on the animation you choose, file size and quality. 

## Output Options
*It is recommended that you use `300x450` and `Best` output quality, as this will give you a good balance between file size and image quality.* If you choose `600x900` and `Best` quality, the file size will still be reasonable, but it will take a __long__ time to render: about 6-7 minutes for a detailed animation such as `Wisps`!*

### File Sizes
- **300x450**: Recommended. 4x smaller output size and much faster rendering compared to `600x900`
- **600x900**: 4x slower and 4x larger than `300x450`. Use sparingly or if you really need high quality on your 1440p+ display. 

### File Quality
- **Good**: Reduces color palette to 255 Colors with moderate dithering.
    - Fastest rendering speed
    - Highly variable file size
- **High**: Reduces color palette to 255 Colors, with low dithering. Causes severe color banding if there are too many gradients in the image
    - Average rendering speed
    - Often produces extremely small file sizes
- **Best**: Full color palette, no loss of quality. Recommended for all use cases if you can stand the render times (approx 1.5 minutes @ `300x450` and 6 minutes @ `600x900`) 
    - Slow rendering speed
    - Reasonably small file sizes, often between `Good` and `High`

## Examples


![](https://i.imgur.com/DerM9Fy.png)
