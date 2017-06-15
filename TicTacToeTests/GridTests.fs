namespace TicTacToeTests

    module GridTests =

        open NUnit.Framework
        open FsUnit
        
        open TicTacToe
                
        let checkListForId (list:List<Space>) id =
                list |> List.find(fun i -> i.Id = id)


        [<Test>]
        let ``Grid is created with 9 spaces`` () =
            let grid = new Grid()
            grid.Spaces.Length |> should equal 9

        [<Test>]
        let ``Grid can retrieve row one`` () =
            let grid = new Grid()
            grid.RowOne.Length |> should equal 3

            let space id =
                checkListForId grid.RowOne id        

            (space 1).Id |> should equal 1
            (space 2).Id |> should equal 2
            (space 3).Id |> should equal 3

        [<Test>]
        let ``Grid can retrieve row Two`` () =
            let grid = new Grid()
            grid.RowTwo.Length |> should equal 3
            
            let space id =
                checkListForId grid.RowTwo id        

            (space 4).Id |> should equal 4
            (space 5).Id |> should equal 5
            (space 6).Id |> should equal 6

        [<Test>]
        let ``Grid can retrieve row Three`` () =
            let grid = new Grid()
            grid.RowThree.Length |> should equal 3
            
            let space id =
                checkListForId grid.RowThree id        

            (space 7).Id |> should equal 7
            (space 8).Id |> should equal 8
            (space 9).Id |> should equal 9

        [<Test>]
        let ``Grid can retrieve column One`` () =
            let grid = new Grid()
            grid.ColumnOne.Length |> should equal 3
            
            let space id =
                checkListForId grid.ColumnOne id        

            (space 1).Id |> should equal 1
            (space 4).Id |> should equal 4
            (space 7).Id |> should equal 7

        [<Test>]
        let ``Grid can retrieve column Two`` () =
            let grid = new Grid()
            grid.ColumnTwo.Length |> should equal 3
            
            let space id =
                checkListForId grid.ColumnTwo id        

            (space 2).Id |> should equal 2
            (space 5).Id |> should equal 5
            (space 8).Id |> should equal 8

        [<Test>]
        let ``Grid can retrieve column Three`` () =
            let grid = new Grid()
            grid.ColumnThree.Length |> should equal 3
            
            let space id =
                checkListForId grid.ColumnThree id        

            (space 3).Id |> should equal 3
            (space 6).Id |> should equal 6
            (space 9).Id |> should equal 9

        [<Test>]
        let ``Grid can retrieve Left Diagonal`` () =
            let grid = new Grid()
            grid.LeftDiagonal.Length |> should equal 3
            
            let space id =
                checkListForId grid.LeftDiagonal id        

            (space 1).Id |> should equal 1
            (space 5).Id |> should equal 5
            (space 9).Id |> should equal 9

        [<Test>]
        let ``Grid can retrieve Right Diagonal`` () =
            let grid = new Grid()
            grid.RightDiagonal.Length |> should equal 3
            
            let space id =
                checkListForId grid.RightDiagonal id        

            (space 3).Id |> should equal 3
            (space 5).Id |> should equal 5
            (space 7).Id |> should equal 7

        [<Test>]
        let ``Grid can retreive single space by id`` () =
            let grid = new Grid()
            let space = 
                grid.GetSpace 1
            
            space.Id |> should equal 1
            space.Mark |> should equal '_'

