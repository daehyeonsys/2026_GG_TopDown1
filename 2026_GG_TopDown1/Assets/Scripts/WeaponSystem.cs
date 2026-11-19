
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    [Header("무기 획득 상태")]
    public bool hasPipe = false;
    public bool hasPistol = false;

    [Header("현재 장착 무기")]
    public int equippedWeapon = 0;

    [Header("대화 시스템")]
    public DialogueBox dialogueBox;

    [Header("쇠파이프 - 아래")]
    public Sprite[] pipeDown = new Sprite[3];

    [Header("쇠파이프 - 위")]
    public Sprite[] pipeUp = new Sprite[3];

    [Header("쇠파이프 - 왼쪽")]
    public Sprite[] pipeLeft = new Sprite[3];

    [Header("쇠파이프 - 오른쪽")]
    public Sprite[] pipeRight = new Sprite[3];

    [Header("권총 - 아래")]
    public Sprite[] pistolDown = new Sprite[3];

    [Header("권총 - 위")]
    public Sprite[] pistolUp = new Sprite[3];

    [Header("권총 - 왼쪽")]
    public Sprite[] pistolLeft = new Sprite[3];

    [Header("권총 - 오른쪽")]
    public Sprite[] pistolRight = new Sprite[3];

    void Update()
    {
        Keyboard kb = Keyboard.current;

        if (kb == null)
            return;

        if (dialogueBox != null && dialogueBox.isOpen)
            return;

        if (kb.digit3Key.wasPressedThisFrame ||
            kb.numpad3Key.wasPressedThisFrame)
        {
            EquipPipe();
        }

        if (kb.digit4Key.wasPressedThisFrame ||
            kb.numpad4Key.wasPressedThisFrame)
        {
            EquipPistol();
        }
    }

    public void GetWeapons()
    {
        if (HasAllWeapons())
            return;

        hasPipe = true;
        hasPistol = true;

        Debug.Log("쇠파이프와 권총 획득!");
    }

    public void EquipPipe()
    {
        if (!hasPipe)
            return;

        equippedWeapon = 3;
        Debug.Log("쇠파이프 장착");
    }

    public void EquipPistol()
    {
        if (!hasPistol)
            return;

        equippedWeapon = 4;
        Debug.Log("권총 장착");
    }

    public void EquipNormal()
    {
        equippedWeapon = 0;
    }

    public bool HasAllWeapons()
    {
        return hasPipe && hasPistol;
    }

    public Sprite[] GetSprites(int direction)
    {
        if (equippedWeapon == 3)
        {
            switch (direction)
            {
                case 0: return pipeDown;
                case 1: return pipeUp;
                case 2: return pipeLeft;
                case 3: return pipeRight;
            }
        }

        if (equippedWeapon == 4)
        {
            switch (direction)
            {
                case 0: return pistolDown;
                case 1: return pistolUp;
                case 2: return pistolLeft;
                case 3: return pistolRight;
            }
        }

        return null;
    }
}
