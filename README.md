<p align="center"><img src="Media/Banner.svg" height="250"></p>

<p align="center"><b>Generate APNG Images for Steam Game Covers.</b></p>

-----

## Prerequisites
- .NET Framework 4.7.2 (Included with Windows 10 1803+ by default)
- [FFMPEG](https://www.ffmpeg.org/) - Needs to be installed and [added to your PATH Environment Variable](https://www.thewindowsclub.com/how-to-install-ffmpeg-on-windows-10), or place `ffmpeg.exe` in the same folder as `SteamAPNG.exe`
## Installation
- Download the [latest release](https://github.com/IridiumIO/SteamAPNG/releases) of SteamAPNG
- Download the effects you want from [here](/Effects) and place them in an `Effects` folder next to the SteamAPNG.exe file.  
    *__Effects aren't included by default as the total download size would be ~250MB__*

## Usage
![](https://i.imgur.com/pykwynw.png)
1. Click on the Preview pane to select an image to apply an effect to. Images should ideally be `300x450` or `600x900` in size; other sizes will be scaled up or down automatically on render.
2. Use the sliders to transform the effect to your liking.  
4. Choose output quality and size (See Output options below), select a filename and hit save.
    - Note: It could take up to 7 minutes to render if you choose a detailed animation at high quality. 

## Output Options
*It is recommended that you use `300x450` and `Best` output quality, as this will give you a good balance between file size and image quality.* If you choose `600x900` and `Best` quality, the file size will still be reasonable, but it will take a __long__ time to render: about 6-7 minutes for a detailed animation such as `Wisps`!*

### File Sizes
- **300x450**: Recommended.
- **600x900**: 4x slower to render and 4x larger than `300x450`. Use sparingly or if you really need high quality on your 1440p+ display. 

### File Quality
- **Good**: Reduces color palette to 255 Colors with moderate dithering.
    - Fastest rendering speed
    - Highly variable file size
- **High**: Reduces color palette to 255 Colors, with low dithering. Can cause noticeable color banding if there are too many gradients in the source image. 
    - Average rendering speed
    - Often produces extremely small file sizes
- **Best**: Full color palette, no loss of quality. Recommended
    - Slow rendering speed (approx 1.5 minutes @ `300x450`)
    - Reasonably small file sizes, often between `Good` and `High`

## Postprocessing

### Reducing Filesize.
Do the following in order: 
1. Compress with [TinyPNG](https://tinypng.com/). If your file is larger than the 5MB threshold, use their API to do it (You can compress 500 images free per month) 
2. Optimize with [APNG Optimizer](https://sourceforge.net/projects/apng/files/APNG_Optimizer/) using either `7ZIP` or `Zopfli` compression. 

## Examples

| ![](https://i.imgur.com/P29a8G3.png) |![](https://i.imgur.com/BajNQ9H.png)  |![](https://i.imgur.com/OrSkURG.png)|![](Effects/Examples/Wisps.png)|
|:--:|:--:|:--:|:--:|
| Good: 1.9MB | High: 825KB | Best: 3.88MB|Post-Optimized High: 588KB|

### See [here](Effects/Examples) for examples of all the built-in effects. 
