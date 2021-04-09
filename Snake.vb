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
    Dim snakeLength As Int16 = 4
    Dim cherry As Point
    Const boardSize As Int16 = 16
    Dim gameStop As Boolean = False

    Sub StartGame()
        Randomize()
        Dim g As Graphics = PictureBox1.CreateGraphics
        GameBoard()
        snakeHead = New Point(8, 8)
        snakeLength = 4
        cherry = New Point(Rnd(1) * boardSize, Rnd(1) * boardSize)
        g.FillRectangle(New SolidBrush(Color.OrangeRed), CInt(PictureBox1.Width * (cherry.X / boardSize)), CInt(PictureBox1.Height * (cherry.Y / boardSize)), CInt(PictureBox1.Width / boardSize), CInt(PictureBox1.Height / boardSize))
    End Sub

    Sub snakeDraw()
        Dim g As Graphics = PictureBox1.CreateGraphics
        If gameStop = False Then
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
            g.FillRectangle(Brushes.Blue, CInt(PictureBox1.Width * (snakeHead.X / boardSize)), CInt(PictureBox1.Height * (snakeHead.Y / boardSize)), CInt(PictureBox1.Width / boardSize), CInt(PictureBox1.Height / boardSize))

            snakeBody.Insert(0, snakeHead)

            If snakeBody.Count > snakeLength Then
                'Erease thing
                Dim rattle As Point = snakeBody.Last()
                g.FillRectangle(New SolidBrush(Me.BackColor), CInt(PictureBox1.Width * (rattle.X / boardSize)), CInt(PictureBox1.Height * (rattle.Y / boardSize)), CInt(PictureBox1.Width / boardSize), CInt(PictureBox1.Height / boardSize))

                snakeBody.Remove(rattle)
            End If
        End If
        Collision()
    End Sub

    Sub Collision()
        Dim g As Graphics = PictureBox1.CreateGraphics
        If snakeHead = cherry Then
            snakeLength += 1
            cherry = New Point(Rnd(1) * boardSize, Rnd(1) * boardSize)
        End If
        g.FillRectangle(New SolidBrush(Color.OrangeRed), CInt(PictureBox1.Width * (cherry.X / boardSize)), CInt(PictureBox1.Height * (cherry.Y / boardSize)), CInt(PictureBox1.Width / boardSize), CInt(PictureBox1.Height / boardSize))

        If boardSize - 1 < snakeHead.X Or boardSize - 1 < snakeHead.Y Or 0 > snakeHead.X Or 0 > snakeHead.Y Then
            gameStop = True
        End If

        For i = 1 To snakeBody.Count
            If snakeHead = snakeBody.Item(i) Then
                gameStop = True
            End If
        Next
    End Sub

    Sub GameBoard()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim boardPen As Pen = New Pen(Color.Black)

        For i = 0 To boardSize
            g.DrawLine(boardPen, CSng(PictureBox1.Width * (i / boardSize)), 0, CSng(PictureBox1.Width * (i / boardSize)), CSng(PictureBox1.Height))
            g.DrawLine(boardPen, 0, CSng(PictureBox1.Height * (i / boardSize)), CSng(PictureBox1.Width), CSng(PictureBox1.Height * (i / boardSize)))
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
                StartGame()

        End Select
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        snakeDraw()

    End Sub
End Class
