module Utilities

type Change<'left, 'right> =
| Addition of 'right
| Update of 'left * 'right
| Deletion of 'left

let compareSequences<'left, 'right> (left: 'left seq) (right: 'right seq) (compare: 'left -> 'right -> int): Change<'left, 'right> seq =
    seq {
        let leftEnumerator = left.GetEnumerator()
        let rightEnumerator = right.GetEnumerator()

        let mutable leftActive = leftEnumerator.MoveNext()
        let mutable rightActive = rightEnumerator.MoveNext()

        while leftActive || rightActive do
            
            while leftActive && ((not rightActive) || (compare leftEnumerator.Current rightEnumerator.Current < 0)) do
                yield Deletion leftEnumerator.Current
                leftActive <- leftEnumerator.MoveNext()

            while rightActive && ((not leftActive) || (compare leftEnumerator.Current rightEnumerator.Current > 0)) do
                yield Addition rightEnumerator.Current
                rightActive <- rightEnumerator.MoveNext()

            while leftActive && rightActive && (compare leftEnumerator.Current rightEnumerator.Current = 0) do
                yield Update (leftEnumerator.Current, rightEnumerator.Current)
                leftActive <- leftEnumerator.MoveNext()
                rightActive <- rightEnumerator.MoveNext()
    }