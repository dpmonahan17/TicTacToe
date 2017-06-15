namespace TicTacToeTests

    module PlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    
        [<Test>]
        let ``Player should have a Mark`` () =
            let player = Player("Ezekiel", 'x')
            player.Mark |> should equal 'x'

        [<Test>]
        let ``Player should have a Name`` () =
            let player = Player("Ezekiel", 'x')
            player.Name |> should equal "Ezekiel"