using System;
using UnityEngine;

public class PuzzlesManager : MonoBehaviour
{
    private GameObject[] _allPuzzles = new GameObject[6]{null,null,null,null,null,null};

    public Action<int> PuzzleSolvedAction;

    public void RegisterPuzzle(int idPuzzle, GameObject puzzle)
    {
        _allPuzzles[idPuzzle] = puzzle;

        PuzzleNotifier puzzleNotifier = puzzle.GetComponent<PuzzleNotifier>();
        if (puzzleNotifier)
        {
            puzzleNotifier.PuzzleSolvedAction += HandlePuzzleSolved;            
        }
    }

    public void ActivatePuzzle(int idPuzzle)
    {

        GameObject puzzle = _allPuzzles[idPuzzle];
        ActivatableBehaviour activatableBehaviour = puzzle.GetComponent<ActivatableBehaviour>();        
        if (activatableBehaviour)
        {
            activatableBehaviour.Activate();
        }
    }

    

    private void HandlePuzzleSolved(int id)
    {
        PuzzleSolvedAction?.Invoke(id);
    }

    void OnDestroy()
    {
        for(int i =0; i<_allPuzzles.Length; i++)
        {
            GameObject puzzle = _allPuzzles[i];

            if (puzzle)
            {
                PuzzleNotifier puzzleNotifier = puzzle.GetComponent<PuzzleNotifier>();
                if (puzzleNotifier)
                {
                    puzzleNotifier.PuzzleSolvedAction -= HandlePuzzleSolved;
                }    
            }
            
        }
    }

}
