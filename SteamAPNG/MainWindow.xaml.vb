Imports System.Windows.Threading

Class MainWindow

    Dim effectspath = IO.Path.Combine(CurDir, "Effects")

    Dim timer As New DispatcherTimer(DispatcherPriority.Send)
    Dim animationFrames As New List(Of Byte())
    Dim bg As BitmapImage

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        AddHandler timer.Tick, AddressOf ElapsedT
        timer.Interval = TimeSpan.FromMilliseconds(33)
        Dim dInfo = New IO.DirectoryInfo(effectspath)
        If Not dInfo.Exists Then
            dInfo.Create()
        End If
        Dim files As List(Of IO.FileInfo) = dInfo.EnumerateFiles("*.fx", IO.SearchOption.AllDirectories).ToList

        files = files.OrderBy(Function(x) x.Name).ToList()

        For Each fx In files
            FXItems.Items.Add(fx)
        Next

    End Sub


    Dim count = 0
    Private Sub ElapsedT()
        If count = animationFrames.Count Then count = 0
        imgbox1.Source = ImageFromBytes(animationFrames(count))

        count += 1
    End Sub


    Private Sub InputFPS_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
        timer.Interval = TimeSpan.FromMilliseconds(1000 / inputFPS.Value)
    End Sub

    Public Function Merge(bgImg, overlayFrame, counter) As Byte()

        Dim tra As New TranslateTransform(positionH.Value, positionV.Value)
        Dim rot As New RotateTransform(rotate.Value, overlayFrame.Width / 2, overlayFrame.Height / 2)
        Dim sct As New ScaleTransform(scalex.Value, scaley.Value, overlayFrame.Width / 2, overlayFrame.Height / 2)

        Dim visual As New DrawingVisual

        Using ctx = visual.RenderOpen
            ctx.DrawImage(bgImg, New Rect(New Size(600, 900)))

            ctx.PushTransform(tra)
            ctx.PushTransform(rot)
            ctx.PushTransform(sct)
            ctx.PushOpacity(opacity.Value)
            ctx.DrawImage(overlayFrame, New Rect(0, 0, overlayFrame.Width, overlayFrame.Height))
            ctx.Pop()
            ctx.Pop()
            ctx.Pop()
            ctx.Pop()
        End Using

        Dim bitmap As RenderTargetBitmap = New RenderTargetBitmap(600, 900, 96, 96, PixelFormats.Pbgra32)
        bitmap.Render(visual)
        bitmap.Freeze()

        Dim pngRend As New PngBitmapEncoder
        pngRend.Frames.Add(BitmapFrame.Create(bitmap))
        Dim ms As New IO.MemoryStream()
        pngRend.Save(ms)
        ms.Seek(0, IO.SeekOrigin.Begin)
        statusLbl.Text = $"Generating Frames...{counter}/{animationFrames.Count}"

        Return ms.ToArray()

    End Function

    Dim combinedImages As New List(Of Byte())

    Private Sub WriteImages()
        Dim i As Integer = 0
        For Each img In animationFrames
            Dim CombImg = Dispatcher.Invoke(Function()
                                                Return Merge(bg, ImageFromBytes(img), i)
                                            End Function)
            combinedImages.Add(CombImg)
            i += 1
        Next
        i = 0
    End Sub

    Private Async Sub SaveBTN_Click(sender As Object, e As RoutedEventArgs)

        If outputPathBox.Text = "" Then
            MsgBox("Output file not specified")
            Return
        End If

        statusLbl.Text = "Generating Frames..."

        SaveBTN.IsEnabled = False
        EffectOptionsBox.IsEnabled = False
        SaveOptionsBox.IsEnabled = False


        Await Task.Run(Sub()
                           WriteImages()
                       End Sub)

        statusLbl.Text = "Building Output... Please Wait"

        Await MakeVideo(inputFPS.Value, outputPathBox.Text)
        combinedImages.Clear()

        Debug.WriteLine(outputPathBox.Text.Replace("apng", "png"))

        IO.File.Delete(outputPathBox.Text)
        Rename(outputPathBox.Text.Replace("png", "apng"), outputPathBox.Text)

        statusLbl.Text = "Done!"
        GC.Collect()

        SaveBTN.IsEnabled = True
        EffectOptionsBox.IsEnabled = True
        SaveOptionsBox.IsEnabled = True

    End Sub


    Private Async Function MakeVideo(fps As Integer, outPath As String) As Task

        outPath = outPath.Replace(".png", ".apng")

        Dim paletteflags = ",split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse"
        Dim fSize = 300

        If RdMedQual.IsChecked Then
            paletteflags = ",split[s0][s1];[s0]palettegen=stats_mode=diff[p];[s1][p]paletteuse=dither=bayer:bayer_scale=5:diff_mode=rectangle"
        ElseIf RdHighQual.IsChecked Then
            paletteflags = ""
        End If

        If Rd600.IsChecked Then fSize = 600


        Dim tcs As New TaskCompletionSource(Of Integer)()

        Dim inputArgs = $"-y -framerate {fps} -f image2pipe -i - -r {fps} -plays 0 -vf ""fps={fps},scale={fSize}:-1:flags=lanczos{paletteflags}"" ""{outPath}"""
        Dim process As New Process

        With process.StartInfo
            .FileName = "ffmpeg.exe"
            .Arguments = $"{inputArgs}"
            .UseShellExecute = False
            .CreateNoWindow = False
            .RedirectStandardInput = True
        End With


        Await Task.Run(Sub()
                           process.Start()
                           process.PriorityClass = ProcessPriorityClass.High
                       End Sub)

        Using stream As New IO.BinaryWriter(process.StandardInput.BaseStream)
            For Each img In combinedImages
                stream.Write(img)
            Next
        End Using

        process.WaitForExit()


    End Function



    Private Sub FXItems_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        timer.Stop()
        Dim finfo As IO.FileInfo = e.AddedItems(0)
        animationFrames = ReadZipToBytes(finfo.FullName)
        count = 0
        timer.Start()
    End Sub


    Private Sub previewBox_previewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim fsd As New Microsoft.Win32.OpenFileDialog()
        fsd.Filter = "PNG Images (*.png)|*.png|All files (*.*)|*.*"
        Console.WriteLine("hello")
        If fsd.ShowDialog = True Then
            bg = New BitmapImage(New Uri(fsd.FileName))
            bgImg.Source = bg
        End If

        Dim rtloc = imgbox1.RenderTransform.Value
        Console.WriteLine(rtloc)

    End Sub

    Private Sub OutputPathButton_Click(sender As Object, e As RoutedEventArgs)
        Dim fsd As New Microsoft.Win32.SaveFileDialog()
        fsd.Filter = "PNG Images (*.png)|*.png|All files (*.*)|*.*"
        fsd.DefaultExt = "png"
        If fsd.ShowDialog = True Then

            outputPathBox.Text = fsd.FileName

        End If
    End Sub

    Private Sub ResetTransforms_Click(sender As Object, e As RoutedEventArgs)
        positionH.Value = 0
        positionV.Value = 0
        scalex.Value = 1
        scaley.Value = 1
        rotate.Value = 0
    End Sub

    Private Sub previewBox_MouseWheel(sender As Object, e As MouseWheelEventArgs)

        If Keyboard.Modifiers = ModifierKeys.Control Then
            If e.Delta > 0 Then
                scaley.Value += 0.2
                scalex.Value += 0.2
            Else
                scaley.Value -= 0.2
                scalex.Value -= 0.2
            End If
        ElseIf Keyboard.Modifiers = ModifierKeys.Alt Then
            If e.Delta > 0 Then
                positionH.Value += 5
            Else
                positionH.Value -= 5
            End If
        ElseIf Keyboard.Modifiers = ModifierKeys.Shift Then
            If e.Delta > 0 Then
                rotate.Value += 5
            Else
                rotate.Value -= 5
            End If
        Else
            If e.Delta > 0 Then
                positionV.Value -= 5
            Else
                positionV.Value += 5
            End If
        End If


    End Sub
End Class