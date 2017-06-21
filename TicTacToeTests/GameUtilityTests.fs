namespace TicTacToeTests

module GameUtilityTests =

    open NUnit.Framework
    open FsUnit

    open TicTacToe

    [<Test>]
    let ``GetScoreForSpace can get the score of one space`` () =
        let gameUtils = GameUtilities()
        let space1 = 
            {Position = (Left, Top); Marked = No}
        let space2 = 
            {Position = (Left, Center); Marked = X}
        let space3 = 
            {Position = (Left, Bottom); Marked = O}
            
        let resultNo = gameUtils.GetScoreForSpace space1
        let resultX = gameUtils.GetScoreForSpace space2
        let resultO = gameUtils.GetScoreForSpace space3

        resultNo |> should equal 0
        resultX |> should equal 1
        resultO |> should equal -1


    [<Test>]
    let ``GetScoreForRow can get the score of one row`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard.grid |> List.filter(fun i -> i.Position = (Left, Top) || i.Position = (Left, Center) || i.Position = (Left, Bottom))
        
        let result = gameUtils.GetScoreForRow grid

        result |> should equal 1



    [<Test>]
    let ``GetPossibleMoves returns grid of valid spaces`` () =
        let utilities = GameUtilities()
        let newGrid : Space List = utilities.GetPossibleMoves FakeSetup.testBoard
        
        newGrid.Length |> should equal 2
        (newGrid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        (newGrid |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No

    [<Test>]
    let ``GetSpace returns specific space`` () =
        let utilities = GameUtilities()
        let space : Space = utilities.GetSpace FakeSetup.testBoard (Middle, Center)
        
        space.Marked |> should equal X
        space.Position |> should equal (Middle, Center)

    [<Test>]
    let ``GetRow returns specific row`` () =
        let utilities = GameUtilities()
        let grid = FakeSetup.testBoard
        
        let result = utilities.GetRow grid 1
        
        (result |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        (result |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No
        (result |> List.find(fun i -> i.Position = (Left, Bottom))).Marked |> should equal X
        
        let result2 = utilities.GetRow grid 4

        (result2 |> List.find(fun i -> i.Position = (Right, Top))).Marked |> should equal O
        (result2 |> List.find(fun i -> i.Position = (Middle, Top))).Marked |> should equal O
        (result2 |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        
    [<Test>]
    let ``UpdateGrid returns Grid with modified space`` () =
        let utilities = GameUtilities()
        let grid = FakeSetup.testBoard
        (grid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
        (grid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal No

        let newGrid = utilities.UpdateGrid FakeSetup.testBoard ({Position = (Left, Top); Marked = X})

        (newGrid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
        (newGrid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal X
        newGrid.grid.Length |> should equal 9
