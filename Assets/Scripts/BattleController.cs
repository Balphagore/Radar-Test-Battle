using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private List<Unit> firstTeam = new List<Unit>();
    [SerializeField]
    private List<Unit> secondTeam = new List<Unit>();
    [SerializeField]
    private bool isStarted;
    [SerializeField]
    private float startTime;

    [Header("References")]
    [SerializeField]
    private Material firstTeamMaterial;
    [SerializeField]
    private Material secondTeamMaterial;

    public delegate void EndBattleHandle(string result);
    public static event EndBattleHandle EndBattleEvent;

    //Подпись и отпись от ивентов.
    private void OnEnable()
    {
        Unit.UnitDestroyEvent += OnUnitDestroyEvent;
        Interface.StartButtonClickEvent += OnStartButtonClickEvent;
    }

    private void OnDisable()
    {
        Unit.UnitDestroyEvent -= OnUnitDestroyEvent;
        Interface.StartButtonClickEvent -= OnStartButtonClickEvent;
    }

    //Окрашивание юнитов в цвета команд.
    private void Start()
    {
        foreach (var unit in firstTeam)
        {
            unit.UnitRenderer.material = firstTeamMaterial;
        }
        foreach (var unit in secondTeam)
        {
            unit.UnitRenderer.material = secondTeamMaterial;
        }
    }

    //Если битва начата - нахождение ближайших противников для каждого юнита каждой из команд
    private void Update()
    {
        if (isStarted)
        {
            FindTargets(firstTeam, secondTeam);
            FindTargets(secondTeam, firstTeam);
        }
    }

    //Функция для всей команды проверяет расстояния до всех противников и выбирает ближайшего, устанавливая его целью юнита.
    private void FindTargets(List<Unit> units, List<Unit> targets)
    {
        for (int i = 0; i < units.Count; i++)
        {
            float minDistance = float.PositiveInfinity;
            int targetIndex = -1;
            for (int j = 0; j < targets.Count; j++)
            {
                float distance = Vector3.Distance(units[i].transform.position, targets[j].transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    targetIndex = j;
                }
            }
            if (targetIndex > -1)
            {
                units[i].Target = targets[targetIndex];
            }
        }
    }

    //В ответ на ивент об уничтожении юнита он убирается из списка своей команды. Если после этого в списке не остается юнитов, то противоположная команда побеждает.
    private void OnUnitDestroyEvent(Unit unit, Team team)
    {
        switch (team)
        {
            case Team.first:
                firstTeam.Remove(unit);
                if (firstTeam.Count <= 0)
                {
                    EndBattleEvent?.Invoke("Second team win in " + (Time.realtimeSinceStartup - startTime) + " seconds");
                }
                break;
            case Team.second:
                secondTeam.Remove(unit);
                if (secondTeam.Count <= 0)
                {
                    EndBattleEvent?.Invoke("First team win in " + (Time.realtimeSinceStartup - startTime) + " seconds");
                }
                break;
        }
    }

    //В ответ на ивент нажатия кнопки старт запоминается время начала битвы и переключается значение переменной, показывающей - начата битва или нет.
    private void OnStartButtonClickEvent()
    {
        isStarted = true;
        startTime = Time.realtimeSinceStartup;
    }
}
public enum Team
{
    first,
    second
}