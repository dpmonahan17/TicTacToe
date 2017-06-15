namespace TicTacToeTests

    module BoardTests = 

        open NUnit.Framework
        open FsUnit
        open System
        open TicTacToe

        
        [<Test>]
        let ``Board_CanPrintGrid`` () =
            let grid = Grid()
            grid.MarkSpace 4 'x'
            grid.MarkSpace 7 'x'
            
            
            Board.printBoard grid
            1 |> should equal 1

        


                    