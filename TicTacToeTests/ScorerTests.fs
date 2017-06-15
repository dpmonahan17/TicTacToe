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

        let setupSpaceWithMarker (grid:Grid) (mark:char) (s1:int) =
            (grid.GetSpace s1).SetMark mark

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

        [<Test>]
        let ``CheckForWinner_ChecksGrid_ForThreeInARow_ReturnsFalseIfNoRowOfThree`` () =
            let grid = Grid()
            setupSpaceWithMarker grid 'x' 1
            setupSpaceWithMarker grid 'x' 2
            setupSpaceWithMarker grid 'o' 3
            
            Scorer.checkForWinner grid |> should equal false
                        
        [<Test>]
        let ``CheckForWinner_ChecksGrid_ForThreeInARow_ReturnsTrueIfRowOfThreeXs`` () =
            let grid = Grid()
            setupSpaceWithMarker grid 'x' 1
            setupSpaceWithMarker grid 'x' 5
            setupSpaceWithMarker grid 'x' 9
            
            Scorer.checkForWinner grid |> should equal true
                       
        [<Test>]
        let ``CheckForWinner_ChecksGrid_ForThreeInARow_ReturnsTrueIfRowOfThreeOs`` () =
            let grid = Grid()
            setupSpaceWithMarker grid 'o' 3
            setupSpaceWithMarker grid 'o' 5
            setupSpaceWithMarker grid 'o' 7
            
            Scorer.checkForWinner grid |> should equal true

        [<Test>]
        let ``GetWinningMarker_ReturnsXIfThreeXs`` () =
            let grid = Grid()
            setupSpaceWithMarker grid 'x' 1
            setupSpaceWithMarker grid 'x' 5
            setupSpaceWithMarker grid 'x' 9
            
            Scorer.getWinningMarker grid |> should equal 'x'

        [<Test>]
        let ``GetWinningMarker_ReturnsOIfThreeOs`` () =
            let grid = Grid()
            setupSpaceWithMarker grid 'o' 4
            setupSpaceWithMarker grid 'o' 5
            setupSpaceWithMarker grid 'o' 6
            setupSpaceWithMarker grid 'x' 1
            setupSpaceWithMarker grid 'x' 3
            setupSpaceWithMarker grid 'x' 9

            Scorer.getWinningMarker grid |> should equal 'o'