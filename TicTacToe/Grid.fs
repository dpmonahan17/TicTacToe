namespace TicTacToe

// open TicTacToe

// type public Grid() = class

//     let spaces = [Space 1; Space 2; Space 3;
//                   Space 4; Space 5; Space 6;
//                   Space 7; Space 8; Space 9]

//     let getSpaces id1 id2 id3 (spaceList:List<Space>) = 
//         spaceList |> List.filter (fun i -> i.Id = id1 || i.Id = id2 || i.Id = id3)

//     member x.Spaces = 
//         spaces

//     member x.RowOne = 
//         getSpaces 1 2 3 spaces 
            
//     member x.RowTwo = 
//         getSpaces 4 5 6 spaces

//     member x.RowThree = 
//         getSpaces 7 8 9 spaces

//     member x.ColumnOne =
//         getSpaces 1 4 7 spaces

//     member x.ColumnTwo =
//         getSpaces 2 5 8 spaces

//     member x.ColumnThree =
//         getSpaces 3 6 9 spaces

//     member x.LeftDiagonal =
//         getSpaces 1 5 9 spaces

//     member x.RightDiagonal =
//         getSpaces 3 5 7 spaces

//     member x.GetRow (row:int) =
//         match row with
//         | 1 -> x.RowOne
//         | 2 -> x.RowTwo
//         | 3 -> x.RowThree
//         | 4 -> x.ColumnOne
//         | 5 -> x.ColumnTwo
//         | 6 -> x.ColumnThree
//         | 7 -> x.LeftDiagonal
//         | 8 -> x.RightDiagonal

//     member x.GetSpace id =
//         spaces |> List.find(fun i -> i.Id = id)

//     member x.CheckIfFull =
//         spaces |> List.filter(fun i -> i.Mark = '_') |> List.isEmpty

//     member x.MarkSpace id mark =
//         (x.GetSpace id).SetMark mark

//     member x.GetMark id =
//         (x.GetSpace id).Mark

// end