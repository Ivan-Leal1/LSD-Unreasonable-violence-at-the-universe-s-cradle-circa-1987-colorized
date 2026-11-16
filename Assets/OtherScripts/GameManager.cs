using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    //actions: 0 = melee attack; 1 = ranged attack; 2 = inventory; 3 = talk;
    [Header("Miscellanea")]
    int _selectedAction = 0;
    int _telephoneRotation = 0;
    int _needleRotation = -90;
    int _textCounter;
    bool _returnToCombatMenu;
    bool _dead = false;
    [Header("Stats")] //TODO: CHANGE THESE WHEN WEAPONS ARE IMPLEMENTED
    public int _meleeDamage = 1;
    public int _meleeAccuracy = 90;
    public int _rangedDamage = 2;
    public int _rangedAccuracy = 85;
    int _ammo = 6;
    int _maxAmmo = 6;
    int _currentMeleeDamage = 1;
    int _currentMeleeAccuracy = 90;
    int _currentRangedDamage = 2;
    int _currentRangedAccuracy = 85;
    int _hp = 6;
    int _maxHp = 6;
    // currently open menu.
    [Header("Menus")]
    bool combat = false;
    bool inventory = false;
    bool overworld = true;
    bool text = false;
    [Header("Publics and serialize fields")]
    [SerializeField] GameObject _telephone;
    [SerializeField] GameObject _needle;
    [SerializeField] GameObject _inventoryMenu;
    [SerializeField] GameObject _textBoxMenu;
    [SerializeField] TMP_Text _inventoryItemDescription;
    [SerializeField] TMP_Text _textBoxText;
    [SerializeField] Image _inventoryItemImage;
    [SerializeField] List<GameObject> _dontDestroyOnLoadObjects;
    GameObject _enemy;
    List<RoomBase> _roomOptions;
    public List<ItemBase> _itemsHeld;
    public List<string> _textList;
    [Header("Screens")]
    [SerializeField] TMP_Text _actionInfoScreenText;
    [SerializeField] TMP_Text _roomInfoScreenText;

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
                        Destroy(_enemy);
                        combat = false;
                        overworld = true;
                        _telephone.SetActive(false);
                        _selectedAction = 0;
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
                    _inventoryMenu.SetActive(false);
                    _telephone.SetActive(true);
                    _selectedAction = 2;
                } else if (_selectedAction != 0) {
                    _selectedAction -= 1;
                }
                _inventoryItemDescription.text = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetDesc();
                _inventoryItemImage.sprite = _itemsHeld[_selectedAction].GetComponent<ItemBase>().GetImg();
            } else if (overworld) {
                _currentMeleeAccuracy = _meleeAccuracy;
                _currentMeleeDamage = _meleeDamage;
                _currentRangedAccuracy = _rangedAccuracy;
                _currentRangedDamage = _rangedDamage;
                _roomOptions[_selectedAction].OnUse();
                if (_textList.Count != 0) {
                    //assume event
                    _textCounter = -1;
                    Next();
                }
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
            } else if (overworld) {
                if (_selectedAction == 0) {
                    _selectedAction = 2;
                } else {
                    _selectedAction -= 1;
                }
                WriteToRoomInfoScreen(_roomOptions[_selectedAction].Desc());
                //TODO: Show door at screen center, write desc at top right tv
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
            } else if (overworld) {
                if (_selectedAction == 2) {
                    _selectedAction = 0;
                } else {
                    _selectedAction += 1;
                }
                WriteToRoomInfoScreen(_roomOptions[_selectedAction].Desc());
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
        /*if (change == 1) {
            _needleRotation -= 30;
        } else if (change == 2) {
            _needleRotation -= 60;
        }  else if (change < 0) {
            _needleRotation += 30;
        }
        if (_needleRotation < -90) {
            _needleRotation = -90;
        }*/
        if (change > 0) {
            for (int i = 0; i < change; i++) {
                Debug.Log("Triggered");
                _needleRotation -= 30;
            }
        } else if (change < 0) {
            Debug.Log("Haii");
            for (int i = 0; i > change; i--) {
                Debug.Log("Triggered" + i);
                _needleRotation += 30;
            }
        }
        if (_hp <= 0) {
            _dead = true;
            _needleRotation = 90;
            _textList.Add("You have been defeated");
        } else if (_hp > _maxHp) {
            _needleRotation = -90;
            _hp = _maxHp;
        }
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

    public void ReceiveRooms(List<RoomBase> rooms) {
        _roomOptions = rooms;
        _selectedAction = 0;
        WriteToRoomInfoScreen(_roomOptions[0].Desc());
        //TODO: show door at center, show info at top right tv
    }

    public void ReceiveEnemy(GameObject enemy) {
        _enemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
        _selectedAction = 0;
        //TODO: show enemy at center
        overworld = false;
        combat = true;
        _telephone.SetActive(true);
        _telephoneRotation = 0;
        WriteToActionScreen();
    }

    public void Victory() {
        _textCounter = -1;
        _returnToCombatMenu = false;
        combat = false;
        _telephone.SetActive(false);
        Next();
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
        if (_ammo > 0) {
            _ammo -= 1;
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
        if (_enemy.GetComponent<EnemyStats>().CurrentHp() > 0) {
            _enemy.GetComponent<EnemyStats>().UseSkill();
            _textCounter = -1;
            _returnToCombatMenu = true;
            Next();
        }
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
            if (_dead) {
            foreach (GameObject i in _dontDestroyOnLoadObjects) {
                Destroy(i);
            }
            SceneManager.LoadScene(3);
            Destroy(gameObject);
            } else if (_returnToCombatMenu) {
                combat = true;
                text = false;
                _telephone.SetActive(true);
                _textBoxMenu.SetActive(false);
            } else { //assume overworld
                overworld = true;
                text = false;
                _textBoxMenu.SetActive(false);
            }
            WriteToActionScreen();
            _textList.Clear();
        } else {
            _textBoxText.text = _textList[_textCounter];
        }
    }

    private void WriteToActionScreen() {
        if (combat) {
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
        } else {
            _actionInfoScreenText.text = "Waiting for input...";
        }
    }

    private void WriteToRoomInfoScreen(string text) {
        if (overworld) {
            _roomInfoScreenText.text = text;
        }
    }

    private void Update() {
        if (_telephone.transform.rotation != Quaternion.Euler(0, 0, _telephoneRotation)) {
            _telephone.transform.rotation = Quaternion.Lerp(_telephone.transform.rotation, Quaternion.Euler(0, 0, _telephoneRotation), 10 * Time.deltaTime);            
        }
        if (_needle.transform.rotation != Quaternion.Euler(0, 0, _needleRotation)) {
            _needle.transform.rotation = Quaternion.Lerp(_needle.transform.rotation, Quaternion.Euler(0, 0, _needleRotation), 10 * Time.deltaTime);
        }
    }
    
    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            foreach (GameObject i in _dontDestroyOnLoadObjects) {
                DontDestroyOnLoad(i);
            }
            SceneManager.LoadScene(2);
        } else if (Singleton != this) {
            Destroy(gameObject);
        }
        WriteToActionScreen();
    }
}