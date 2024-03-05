using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonManager : MonoBehaviour
{
    public float count;
    public string _id;  
    public List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();
    public List<GameObject> selectPokemons = new List<GameObject>();

    public Color normalColor;
    public Color newColor;
}
