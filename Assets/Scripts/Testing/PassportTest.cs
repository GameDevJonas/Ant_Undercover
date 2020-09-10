using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UI;

public class PassportTest : MonoBehaviour
{
    public PlayerTeamTest playerParent;

    public RawImage passBack;
    public TextMeshProUGUI nameText, roleText;

    public List<Texture> passports;
    public string[] names;
    public string[] roleName;
    public Color[] roleColors;

    void Start()
    {
        //playerParent = GetComponentInParent<PlayerTeamTest>();
        switch (playerParent.myTeam)
        {
            case TeamTesting.PlayerTeams.police:
                MakePassPort(0);
                break;
            case TeamTesting.PlayerTeams.civillian:
                MakePassPort(1);
                break;
            case TeamTesting.PlayerTeams.spy:
                MakePassPort(2);
                break;
        }
    }

    void MakePassPort(int role)
    {
        passBack.texture = passports[role];
        roleText.text = roleName[role];
        roleText.color = roleColors[role];
        nameText.text = names[Random.Range(0, names.Length + 1)];
    }
}
