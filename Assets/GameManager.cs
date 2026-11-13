using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    //actions: 0 = melee attack; 1 = ranged attack; 2 = inventory; 3 = talk;
    [Header("Miscellanea")]
    int _selectedAction = 0;
    int _telephoneRotation = 0;
    int _textCounter;
    bool _returnToCombatMenu;
    [Header("Stats")] //TODO: CHANGE THESE WHEN WEAPONS ARE IMPLEMENTED
    int _meleeDamage = 1;
    int _meleeAccuracy = 90;
    int _rangedDamage = 2;
    int _rangedAccuracy = 85;
    int _ammo = 6;
    int _maxAmmo = 6;
    int _currentMeleeDamage = 1;
    int _currentMeleeAccuracy = 90;
    int _currentRangedDamage = 2;
    int _currentRangedAccuracy = 85;
    int _hp = 4;
    int _maxHp = 4;
    // currently open menu.
    [Header("Menus")]
    bool combat = true;
    bool inventory = false;
    bool overworld = false;
    bool text = false;
    [Header("Publics and serialize fields")]
    [SerializeField] GameObject _telephone;
    [SerializeField] GameObject _inventoryMenu;
    [SerializeField] GameObject _textBoxMenu;
    [SerializeField] TMP_Text _actionInfoScreenText;
    [SerializeField] TMP_Text _inventoryItemDescription;
    [SerializeField] TMP_Text _textBoxText;
    [SerializeField] Image _inventoryItemImage;
    public GameObject _enemy;
    public List<ItemBase> _itemsHeld;
    public List<string> _textList;

    public void OnUse(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (combat) {
                switch (_selectedAction) {
                    case 0:
                        //melee attack
                        AttackM();
                        break;
                    case 1:
                        //melee attack
                        AttackR();
                        break;
                    case 2:
                        //inventory
                        Debug.Log("Inventory");
                        if (_itemsHeld.Count != 0) {
                            combat = false;
                            inventory = true;
                            _selectedAction = 0;
                            _telephone.SetActive(false);
                            _inventoryMenu.SetActive(true);
                            _inventoryItemDescription.text = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetDesc();
                            _inventoryItemImage.sprite = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetImg();
                        }
                        break;
                    case 3:
                        //run
                        Debug.Log("Run");
                        break;
                    case 4:
                        //talk
                        Talk();
                        break;
                    default:
                        break;
                }
            } else if (inventory) {
                _itemsHeld[_selectedAction].OnUse();
                _itemsHeld.RemoveAt(_selectedAction);
                if (_itemsHeld.Count == 0) {
                    inventory = false;
                    combat = true;
                    _selectedAction = 2;
                }
            } else if (overworld) {
                _currentMeleeAccuracy = _meleeAccuracy;
                _currentMeleeDamage = _meleeDamage;
                _currentRangedAccuracy = _rangedAccuracy;
                _currentRangedDamage = _rangedDamage;
            } else if (text) {
                Next();
            }
        }
    }

    public void TurnLeft(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (combat) {
                if (_selectedAction == 0) {
                    _selectedAction = 4;
                } else {
                    _selectedAction -= 1;
                }
                _telephoneRotation -= 72;
                WriteToActionScreen();
            } else if (inventory) {
                if (_selectedAction == 0) {
                    _selectedAction = _itemsHeld.Count - 1;
                } else {
                    _selectedAction -= 1;
                }
                _inventoryItemDescription.text = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetDesc();
                _inventoryItemImage.sprite = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetImg();
            }
            Debug.Log(_selectedAction);
        }
    }
    
    public void TurnRight(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (combat) {
                if (_selectedAction == 4) {
                    _selectedAction = 0;
                } else {
                    _selectedAction += 1;
                }
                _telephoneRotation += 72;
                WriteToActionScreen();
            } else if (inventory) {
                if (_selectedAction == _itemsHeld.Count - 1) {
                    _selectedAction = 0;
                } else {
                    _selectedAction += 1;
                }
                _inventoryItemDescription.text = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetDesc();
                _inventoryItemImage.sprite = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetImg();
            }
            Debug.Log(_selectedAction);
        }
    }

    public void Return(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (inventory) {
                inventory = false;
                combat = true;
                _telephone.SetActive(true);
                _inventoryMenu.SetActive(false);
                _selectedAction = 2;
            }
        }
    }

    public void ChangeHp(int change) {
        _hp += change;
        if (_hp <= 0) {
            Debug.Log("Lose");
        } else if (_hp > _maxHp) {
            _hp = _maxHp;
        }
        Debug.Log("player hp: " + _hp);
    }

    public void ChangeValue(int change, int stat) {
        //0 = melee damage; 1 = melee accuracy; 2 = ranged damage; 3 = ranged accuracy; 4 = ammo
        switch (stat) {
            case 0:
                _currentMeleeDamage += change;
                break;
            case 1:
                _currentMeleeAccuracy += change;
                break;
            case 2:
                _currentRangedDamage += change;
                break;
            case 3:
                _currentRangedAccuracy += change;
                break;
            case 4:
                if (_ammo + change >= _maxAmmo) {
                    _ammo = _maxAmmo;
                } else {
                    _ammo += change;
                }
                break;
            default:
                break;
        }
    }

    private void AttackM() {
        _textList.Clear();
        if (Random.Range(1, 101) <= _currentMeleeAccuracy) {
            Debug.Log("hit");
            _textList.Add("Hit! Dealt " + _currentMeleeDamage + " damage!");
            _enemy.GetComponent<EnemyStats>().TakeDamage(_currentMeleeDamage);
        } else {
            _textList.Add("Miss!");
            Debug.Log("Miss");
        }
        Pass();
    }

    private void AttackR() {
        _textList.Clear();
        _ammo -= 1;
        if (_ammo > 0) {
            if (Random.Range(1, 101) <= _currentRangedAccuracy) {
                Debug.Log("hit");
                _textList.Add("Hit! Dealt " + _currentRangedDamage + " damage!");
                _enemy.GetComponent<EnemyStats>().TakeDamage(_currentRangedDamage);
            } else {
                _textList.Add("Miss!");
                Debug.Log("Miss");
            }
            Pass();
        } else {
            _textList.Add("No ammo!");
            _textCounter = -1;
            _returnToCombatMenu = true;
            Next();
        }
    }

    private void Talk() {
        _textList.Clear();
        _enemy.GetComponent<EnemyStats>().Talk();
        _textCounter = -1;
        _returnToCombatMenu = true;
        Next();
    }

    private void Pass() {
        _enemy.GetComponent<EnemyStats>().UseSkill();
        _textCounter = -1;
        _returnToCombatMenu = true;
        Next();
    }

    private void Next() {
        if (_textCounter == -1) {
            combat = false;
            overworld = false;
            text = true;
            _telephone.SetActive(false);
            _textBoxMenu.SetActive(true);
        }
        _textCounter += 1;
        if (_textCounter == _textList.Count) {
            if (_returnToCombatMenu) {
                combat = true;
                text = false;
                _telephone.SetActive(true);
                _textBoxMenu.SetActive(false);
            } else { //assume overworld
                overworld = true;
                text = false;
                _textBoxMenu.SetActive(false);
            }
        } else {
            _textBoxText.text = _textList[_textCounter];
        }
    }

    private void WriteToActionScreen() {
        switch (_selectedAction) {
            case 0:
                _actionInfoScreenText.text = "Melee attack!\nDamage = " + _currentMeleeDamage + "\nAccuracy = " + _currentMeleeAccuracy;
                break;
            case 1:
                _actionInfoScreenText.text = "Range attack!\nDamage = " + _currentRangedDamage + "\nAccuracy = " + _currentRangedAccuracy + "\nAmmo = " + _ammo;
                break;
            case 2:
                _actionInfoScreenText.text = "Inventory!\nCheck and use your items.\nUsing an item does not end the turn.";
                break;
            case 3:
                _actionInfoScreenText.text = "Run!\nAttempt an escape.";
                break;
            case 4:
                _actionInfoScreenText.text = "Talk!\nAttempt to extract information from your opponent.\nDoes not end your turn.";
                break;
            default:
                break;
        }
    }

    private void Update() {
        if (_telephone.transform.rotation != Quaternion.Euler(0, 0, _telephoneRotation)) {
            _telephone.transform.rotation = Quaternion.Lerp(_telephone.transform.rotation, Quaternion.Euler(0, 0, _telephoneRotation), 10 * Time.deltaTime);            
        }
    }
    
    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);    
        } else if (Singleton != this) {
            Destroy(gameObject);
        }
        WriteToActionScreen();
    }
}