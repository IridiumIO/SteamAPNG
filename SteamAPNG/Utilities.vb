Imports System.IO.Compression

Module Utilities

    Public Function StreamToBytes(stream As IO.Stream) As Byte()

        Using ms As New IO.MemoryStream
            stream.CopyTo(ms)
            Return ms.ToArray
        End Using

    End Function

    Public Function ImageFromBytes(buffer As Byte()) As BitmapImage

        Dim xMem = New IO.MemoryStream(buffer)
        Dim img = New BitmapImage()
        img.BeginInit()
        img.StreamSource = xMem
        img.EndInit()
        img.Freeze()
        Return img

    End Function

    Public Function ReadZipToBytes(zipPath)

        Dim ImgCollection As New List(Of Byte())
        Using inStream = ZipFile.OpenRead(zipPath)
            For Each ex In inStream.Entries
                Dim mx As Byte() = StreamToBytes(ex.Open())
                ImgCollection.Add(mx)
            Next
        End Using
        Return ImgCollection

    End Function



    Public Function ApplyRenderTransform(Action As String)

        Dim tfgroup As New TransformGroup
        Dim sct As New ScaleTransform
        Dim rot As New RotateTransform

        If Action = "FlipH" Then sct.ScaleX = -1
        If Action = "FlipV" Then sct.ScaleY = -1
        If Action = "Rot180" Then rot.Angle = 180

        tfgroup.Children.Add(sct)
        tfgroup.Children.Add(rot)
        Return tfgroup

    End Function


End Module