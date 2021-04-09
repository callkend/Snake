Public Class Snake
    Enum Directions
        UP
        DOWN
        LEFT
        RIGHT
    End Enum

    Dim snakeHead As Point = New Point(8, 8)
    Dim currentDirection As Int16
    Dim snakeBody As List(Of Point) = New List(Of Point)

    Sub snakeDraw()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim lastDirection As Int16


        Select Case currentDirection
            Case Directions.UP
                snakeHead = New Point(snakeHead.X, snakeHead.Y - 1)
            Case Directions.DOWN
                snakeHead = New Point(snakeHead.X, snakeHead.Y + 1)
            Case Directions.LEFT
                snakeHead = New Point(snakeHead.X - 1, snakeHead.Y)
            Case Directions.RIGHT
                snakeHead = New Point(snakeHead.X + 1, snakeHead.Y)
        End Select
        g.FillRectangle(Brushes.Blue, CInt(PictureBox1.Width * (snakeHead.X / 16)), CInt(PictureBox1.Height * (snakeHead.Y / 16)), CInt(PictureBox1.Width / 16), CInt(PictureBox1.Height / 16))

        snakeBody.Insert(0, snakeHead)

        If snakeBody.Count > 4 Then
            'Erease thing
            Dim rattle As Point = snakeBody.Last()
            g.FillRectangle(New SolidBrush(Me.BackColor), CInt(PictureBox1.Width * (rattle.X / 16)), CInt(PictureBox1.Height * (rattle.Y / 16)), CInt(PictureBox1.Width / 16), CInt(PictureBox1.Height / 16))

            snakeBody.Remove(rattle)
        End If
    End Sub
    Sub gameBoard()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim boardPen As Pen = New Pen(Color.Black)

        For i = 0 To 16
            g.DrawLine(boardPen, CSng(PictureBox1.Width * (i / 16)), 0, CSng(PictureBox1.Width * (i / 16)), CSng(PictureBox1.Height))
            g.DrawLine(boardPen, 0, CSng(PictureBox1.Height * (i / 16)), CSng(PictureBox1.Width), CSng(PictureBox1.Height * (i / 16)))
        Next

        boardPen.Dispose()
        g.Dispose()
    End Sub

    Private Sub WASD_Press(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        Dim keyPressed As String = e.KeyChar.ToString
        Select Case keyPressed
            Case "w"
                currentDirection = Directions.UP
            Case "a"
                currentDirection = Directions.LEFT
            Case "s"
                currentDirection = Directions.DOWN
            Case "d"
                currentDirection = Directions.RIGHT
            Case vbCr
                gameBoard()
        End Select
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        snakeDraw()

    End Sub
End Class
