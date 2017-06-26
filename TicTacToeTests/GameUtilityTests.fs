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
    let ``CheckBoardIsFull returns true if any spaces are not marked`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        let grid2 = FakeSetup.fullBoard
        
        let result = gameUtils.CheckBoardIsFull grid
        let result2 = gameUtils.CheckBoardIsFull grid2
        
        result |> should equal false
        result2 |> should equal true

    [<Test>]
    let ``UpdateGrid returns Grid with modified space`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        (grid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
        (grid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal No

        let newGrid = gameUtils.UpdateGrid FakeSetup.testBoard ({Position = (Left, Top); Marked = X})

        (newGrid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
        (newGrid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal X
        newGrid.grid.Length |> should equal 9

    [<Test>]
    let ``GetSpace returns specific space`` () =
        let gameUtils = GameUtilities()
        let space : Space = gameUtils.GetSpace FakeSetup.testBoard (Middle, Center)
        
        space.Marked |> should equal X
        space.Position |> should equal (Middle, Center)

    [<Test>]
    let ``GetRow returns specific row`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        
        let result = gameUtils.GetRow grid 1
        
        (result |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        (result |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No
        (result |> List.find(fun i -> i.Position = (Left, Bottom))).Marked |> should equal X
        
        let result2 = gameUtils.GetRow grid 4

        (result2 |> List.find(fun i -> i.Position = (Right, Top))).Marked |> should equal O
        (result2 |> List.find(fun i -> i.Position = (Middle, Top))).Marked |> should equal O
        (result2 |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No


    [<Test>]
    let ``CheckForXWin returns true if X has met win conditions`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        let winGrid = FakeSetup.XWinBoard

        let result1 = gameUtils.CheckForXWin grid
        let result2 = gameUtils.CheckForXWin winGrid

        result1 |> should equal false
        result2 |> should equal true

    [<Test>]
    let ``CheckForOWin returns true if O has met win conditions`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        let winGrid = FakeSetup.OWinBoard

        let result1 = gameUtils.CheckForOWin grid
        let result2 = gameUtils.CheckForOWin winGrid

        result1 |> should equal false
        result2 |> should equal true               
        
    [<Test>]
    let ``CheckForWin returns true if either X or O has met win conditions`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.testBoard
        let winXGrid = FakeSetup.XWinBoard
        let winOGrid = FakeSetup.OWinBoard

        let result1 = gameUtils.CheckForWin grid
        let result2 = gameUtils.CheckForWin winXGrid
        let result3 = gameUtils.CheckForWin winOGrid

        result1 |> should equal false
        result2 |> should equal true         
        result3 |> should equal true

    [<Test>]
    let ``CheckForTie returns true if board is full and neither X nor O has won`` () =
        let gameUtils = GameUtilities()
        let grid = FakeSetup.fullBoard
        let grid2 = FakeSetup.XWinBoard

        let resultTie = gameUtils.CheckForTie grid
        let resultWin = gameUtils.CheckForTie grid2

        resultTie |> should equal true
        resultWin |> should equal false

    [<Test>]
    let ``GetPossibleMoves returns grid of valid spaces`` () =
        let utilities = GameUtilities()
        let newGrid : Space List = utilities.GetPossibleMoves FakeSetup.testBoard
        
        newGrid.Length |> should equal 2
        (newGrid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        (newGrid |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No


    [<Test>]
    let ``BuildBlankBoard returns a board with no marked spaces`` () =
        let gameUtils = GameUtilities()
        
        let board = gameUtils.BuildBlankBoard

        (board.grid |> List.map(fun i -> i.Marked = No)).Length |> should equal 9

    [<Test>]
    let ``MarkMove returns a space marked with the players mark`` () =
        let gameUtils = GameUtilities()
        let space = {Position = (Left,Center); Marked = O}
        let player = {Name = "test"; Mark = X}
        let newSpace = gameUtils.MarkMove space player

        newSpace.Position |> should equal (Left,Center)
        newSpace.Marked |> should equal X

    
