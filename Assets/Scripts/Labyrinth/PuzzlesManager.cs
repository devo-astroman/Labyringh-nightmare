using System;
using UnityEngine;
using System.Collections.Generic;



public class PuzzlesManager : MonoBehaviour
{
    private List<PuzzleData> _allPuzzles = new List<PuzzleData>();

    public Action<int> PuzzleSolvedAction;
    private int _lastPuzzleSolved = 0;

    public void RegisterPuzzle(PuzzleData pData)
    {
        _allPuzzles.Add(pData);

        PuzzleNotifier puzzleNotifier = pData.Puzzle.GetComponent<PuzzleNotifier>();
        if (puzzleNotifier)
        {
            puzzleNotifier.PuzzleSolvedAction += HandlePuzzleSolved;            
        }

        Puzzle puzzle = pData.Puzzle.GetComponent<Puzzle>();        
        if (puzzle)
        {
            puzzle.SetId(pData.id);
        }
    }

    public void ActivatePuzzle(int idPuzzle)
    {

        PuzzleData puzzleDataToActivate =  _allPuzzles.Find(p =>
        {
            return p.id == idPuzzle;
        });


        GameObject puzzle = puzzleDataToActivate.Puzzle;
        Puzzle activatableBehaviour = puzzle.GetComponent<Puzzle>();        
        if (activatableBehaviour)
        {
            activatableBehaviour.Activate();
        }
    }

    public void DeactivatePuzzle(int idPuzzle)
    {
        PuzzleData puzzleDataToActivate =  _allPuzzles.Find(p =>
        {
            return p.id == idPuzzle;
        });

        GameObject puzzleGo = puzzleDataToActivate.Puzzle;
        Puzzle puzzle = puzzleGo.GetComponent<Puzzle>();        
        if (puzzle)
        {
            puzzle.Deactivate();
        }
    }

    public void ResetPuzzle(int idPuzzle)
    {
        PuzzleData puzzleDataToActivate =  _allPuzzles.Find(p =>
        {
            return p.id == idPuzzle;
        });

        GameObject puzzleGo = puzzleDataToActivate.Puzzle;
        Puzzle puzzle = puzzleGo.GetComponent<Puzzle>();        
        if (puzzle)
        {
            puzzle.Reset();            
        }
    }

    public void ActivateLastPuzzle()
    {
        ActivatePuzzle(_lastPuzzleSolved);
    }

    public void DeactivateAllPuzzles()
    {
        _allPuzzles.ForEach(p =>
        {
            DeactivatePuzzle(p.id);
        });
    }

    public void DeactivateAllPuzzlesBut(int id)
    {
        _allPuzzles.ForEach(p =>
        {
            if(id != p.id)
                DeactivatePuzzle(p.id);
            else
                ActivatePuzzle(p.id);
        });
    }

    void OnDestroy()
    {
        _allPuzzles.ForEach(p =>
        {
            PuzzleNotifier puzzleNotifier = p.Puzzle.GetComponent<PuzzleNotifier>();
            if (puzzleNotifier)
            {
                puzzleNotifier.PuzzleSolvedAction -= HandlePuzzleSolved;
            }   
        });
    }

    private void HandlePuzzleSolved(int id)
    {
        _lastPuzzleSolved = id;
        PuzzleSolvedAction?.Invoke(id);
    }



}
