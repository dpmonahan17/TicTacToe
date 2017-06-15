namespace TicTacToeTests

    module PlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    
        [<Test>]
        let ``Player should have a Mark`` () =
            let player = Player('x')
            player.Mark |> should equal 'x'