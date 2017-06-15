namespace TicTacToeTests

    module GameTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    
        [<Test>]
        let ``Nothing`` () =
            1 |> should equal 1
