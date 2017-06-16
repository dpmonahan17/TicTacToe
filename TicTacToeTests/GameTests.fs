namespace TicTacToeTests

    module GameTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    
        [<Test>]
        let ``Game should perform a turn`` () =
            1 |> should equal 1
        