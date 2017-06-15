namespace TicTacToeTests

    module ScorerTests = 

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        let buildRow c1 c2 c3 =
            let space = Space(1)
            space.SetMark c1
            let space2 = Space(2)
            space2.SetMark c2
            let space3 = Space(3)
            space3.SetMark c3
            [space ; space2 ; space3]

        [<Test>]
        let ``GetValue returns 1 if mark is x`` () =
            Scorer.getValue 'x' |> should equal 1

        [<Test>]
        let ``GetValue returns -1 if mark is o`` () =
            Scorer.getValue 'o' |> should equal -1

        [<Test>]
        let ``GetValue returns 0 if mark is anything else`` () =
            Scorer.getValue 's' |> should equal 0

        [<Test>]
        let ``GetScoreForSpaceReturnsValueForSpace`` () =
            let space = Space(1)
            space.SetMark 'o'
            Scorer.getScoreForSpace space  |> should equal -1

        [<Test>]
        let ``GetScoreForRowReturnsSumofValues`` () =
            let spaces = buildRow 'x' 'x' 'o'            
            Scorer.getScoreForRow spaces  |> should equal 1

        [<Test>]
        let ``GetScoreForRowReturnsThreeIfAllx`` () =
            let spaces = buildRow 'x' 'x' 'x'            
            Scorer.getScoreForRow spaces  |> should equal 3

        [<Test>]
        let ``GetScoreForRowReturnsNegThreeIfAllo`` () =
            let spaces = buildRow 'o' 'o' 'o'            
            Scorer.getScoreForRow spaces  |> should equal -3

            