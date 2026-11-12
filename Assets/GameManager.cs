using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    //actions: 0 = melee attack; 1 = ranged attack; 2 = inventory; 3 = talk;
    [Header("Miscellanea")]
    int _selectedAction = 0;
    int _telephoneRotation = 0;
    [Header("Stats")]
    int _meleeDamage = 1;
    int _meleeAccuracy = 90;
    int _rangedDamage = 2;
    int _rangedAccuracy = 85;
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
    [Header("Publics and serialize fields")]
    [SerializeField] GameObject _telephone;
    public GameObject _enemy;
    public List<ItemBase> _itemsHeld;

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
                        }
                        break;
                    case 3:
                        //talk
                        Talk();
                        break;
                    case 4:
                        Debug.Log("Run");
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
            } else if (inventory) {
                if (_selectedAction == 0) {
                    _selectedAction = _itemsHeld.Count - 1;
                } else {
                    _selectedAction -= 1;
                }
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
            } else if (inventory) {
                if (_selectedAction == _itemsHeld.Count - 1) {
                    _selectedAction = 0;
                } else {
                    _selectedAction += 1;
                }
            }
            Debug.Log(_selectedAction);
        }
    }

    public void Return(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (inventory) {
                inventory = false;
                combat = true;
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
        //0 = melee damage; 1 = melee accuracy; 2 = ranged damage; 3 = ranged accuracy
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
            default:
                break;
        }
    }

    private void AttackM() {
        if (Random.Range(1, 101) <= _currentMeleeAccuracy) {
            Debug.Log("hit");
            _enemy.GetComponent<EnemyStats>().TakeDamage(_currentMeleeDamage);
        } else {
            Debug.Log("Miss");
        }
        Pass();
    }

    private void AttackR() {
        if (Random.Range(1, 101) <= _currentRangedAccuracy) {
            Debug.Log("hit");
            _enemy.GetComponent<EnemyStats>().TakeDamage(_currentRangedDamage);
        } else {
            Debug.Log("Miss");
        }
        Pass();
    }

    private void Talk() {
        _enemy.GetComponent<EnemyStats>().Talk();
    }

    private void Pass() {
        _enemy.GetComponent<EnemyStats>().UseSkill();
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
    }
}