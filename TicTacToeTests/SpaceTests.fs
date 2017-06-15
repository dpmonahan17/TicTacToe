namespace TicTacToeTests


    module SpaceTests = 

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        [<Test>]
        let ``A Space defaults to blank`` () =
            let space = new Space 1
            space.Mark |> should equal '_'
            
        [<Test>]
        let ``A Space can be marked with an x`` () =
            let space = new Space 1
            space.SetMark 'x'
            space.Mark |> should equal 'x'

        [<Test>]
        let ``A Space can be marked with an o`` () =
            let space = new Space 1
            space.SetMark 'o'
            space.Mark |> should equal 'o'

        [<Test>]
        let ``A Space cannot be marked with something else`` () =
            let space = new Space 1
            space.SetMark 's'
            space.Mark |> should equal '_'

