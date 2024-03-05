using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    public string ID;

    [SerializeField] private PokemonManager pokemonManager;
    [SerializeField] private GridSystem gridSystem;

    private void Start()
    {
        pokemonManager = GetComponentInParent<PokemonManager>();
        gridSystem = GetComponentInParent<GridSystem>(); // Tìm đối tượng GridSystem trong scene
    }


    private void OnMouseDown()
    {
        CheckID();
    }

    private void CheckID()
    {
        if (pokemonManager.count % 2 == 0)
        {
            if (pokemonManager._id == "")
            {
                AudioManager.Instance.PlayOnClickPokemon();

                pokemonManager._id = ID;
                DisableCollider();

                pokemonManager.selectPokemons.Add(gameObject);

                gameObject.GetComponent<SpriteRenderer>().color = pokemonManager.newColor;
            }
        }
        else if (pokemonManager.count % 2 == 1)
        {
            GameObject firstPokemon = pokemonManager.selectPokemons[0]; // Lấy Pokemon đầu tiên từ danh sách

            // Kiểm tra xem Pokémon thứ hai có trong danh sách pikachus không
            if (gridSystem.pikachus.Contains(gameObject))
            {
                if (gridSystem.CanConnectPokemons(firstPokemon, gameObject))
                {
                    Debug.Log("Pokemons connected!");
                    // Thực hiện các hành động khác tại đây
                    if (pokemonManager._id != ID)
                    {
                        AudioManager.Instance.PlayFailPokemon();
                        ColorPokemon();
                        OnResetID();
                    }
                    else if (pokemonManager._id == ID)
                    {
                        Debug.Log("Giống nhau và xóa");
                        AudioManager.Instance.PlaySuccessPokemon();
                        OnResetID();
                        Destroy(gameObject, 0.2f);
                        DestroyOtherPokemons(0.2f);
                    }
                    EnableCollider();
                    ClearPokemonList();
                }
                else
                {
                    AudioManager.Instance.PlayFailPokemon();

                    Debug.Log("Pokemons cannot connect!");
                    ColorPokemon();
                    OnResetID();
                    EnableCollider();
                    ClearPokemonList();
                }
            }
            else
            {
                Debug.Log("Pokemon not found in pikachus list!");
                ColorPokemon();
                OnResetID();
                EnableCollider();
                ClearPokemonList();
            }
        }
        pokemonManager.count++;
    }

    private void DestroyOtherPokemons(float delay)
    {
        foreach (GameObject pk in pokemonManager.selectPokemons)
        {
            if (pk != null)
            {
                Destroy(pk, delay);
            }
        }
    }

    private void ClearPokemonList()
    {
        pokemonManager.selectPokemons.RemoveAll(item => item == null);
        pokemonManager.selectPokemons.Clear();
    }

    private void OnResetID()
    {
        pokemonManager._id = "";
    }

    private void EnableCollider()
    {
        foreach (BoxCollider2D colllider in pokemonManager.boxColliders)
        {
            colllider.enabled = true;
        }
        pokemonManager.boxColliders.Clear();
    }
    private void DisableCollider()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        pokemonManager.boxColliders.Add(boxCollider);
    }

    private void ColorPokemon()
    {
        foreach(GameObject pk in pokemonManager.selectPokemons)
        {
            if(pk != null)
            {
                pk.GetComponent<SpriteRenderer>().color = pokemonManager.normalColor;
            }    
        }    
    }    
}
