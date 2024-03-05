using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridSystem : MonoBehaviour
{
    public GameObject pikachuPrefab;
    public GameObject pikachuContainer;
    public BlocksProfileSO blocksProfile;

    public int rows = 9;
    public int columns = 16;
    public float x_spacing = 1.0f;
    public float y_spacing = 1.0f;

    public List<GameObject> pikachus = new List<GameObject>();

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        pikachuContainer.transform.SetParent(transform); // Đặt GameObject cha là con của GridSystem

        Vector3 startPosition = pikachuContainer.transform.position; // Sử dụng transform của GameObject cha

        // Tính toán vị trí bắt đầu của lưới Pikachu
        float startX = startPosition.x - (columns - 1) * x_spacing / 2 + x_spacing; // Chừa ra 1 cột trống ở bên trái
        float startY = startPosition.y + (rows - 1) * y_spacing / 2 - y_spacing; // Chừa ra 1 hàng trống ở bên trên

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Tính toán vị trí của Pikachu trong lưới
                Vector3 spawnPosition = new Vector3(startX + col * x_spacing, startY - row * y_spacing, 0);

                // Kiểm tra xem ô này có phải là ô trống không
                if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1)
                {
                    // Nếu là ô trống thì không cần vẽ Pikachu
                    continue;
                }

                // Tạo đối tượng pikachu
                GameObject pikachu = Instantiate(pikachuPrefab, spawnPosition, Quaternion.identity);
                pikachu.transform.SetParent(pikachuContainer.transform);

                // Áp dụng hiệu ứng scale từ nhỏ đến lớn
                pikachu.transform.localScale = Vector3.zero; // Đặt kích thước ban đầu là zero
                pikachu.transform.DOScale(Vector3.one, 1f); // Scale từ zero tới kích thước ban đầu trong 1 giây

                Pokemon pokemon = pikachu.GetComponent<Pokemon>();
                pikachus.Add(pikachu);

                // Lấy ngẫu nhiên một item từ list items trong BlocksProfileSO
                Item randomItem = blocksProfile.items[Random.Range(0, blocksProfile.items.Count)];

                Debug.Log(randomItem.ID);

                // Gán sprite của item ngẫu nhiên cho Pikachu
                SpriteRenderer spriteRenderer = pikachu.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && randomItem != null)
                {
                    spriteRenderer.sprite = randomItem.sprite;
                    pokemon.ID = randomItem.ID;
                }
            }
        }
    }

    public bool CanConnectPokemons(GameObject pokemon1, GameObject pokemon2)
    {
        // Lấy vị trí của hai Pokémon trong lưới
        Vector3Int pos1 = GetGridPosition(pokemon1.transform.position);
        Vector3Int pos2 = GetGridPosition(pokemon2.transform.position);

        // Mảng để lưu trữ các hướng di chuyển
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        if ((pos1.y == rows - 1 && pos2.y == rows - 1) || (pos1.x == columns - 1 && pos2.x == columns - 1))
        {
            return true;
        }

        // Tạo một danh sách lưu trữ tất cả các ô đã được duyệt
        List<Vector3Int> visited = new List<Vector3Int>();

        // Khởi tạo hàng đợi cho thuật toán BFS
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(pos1);

        // Thực hiện BFS
        while (queue.Count > 0)
        {
            Vector3Int currentPos = queue.Dequeue();

            // Kiểm tra xem có đến được Pokemon thứ hai không
            if (currentPos == pos2)
            {
                return true;
            }

            // Duyệt qua các ô kề
            for (int i = 0; i < 4; i++)
            {
                int nx = currentPos.x + dx[i];
                int ny = currentPos.y + dy[i];

                // Kiểm tra xem ô kề có hợp lệ không
                if (nx >= 0 && nx < columns && ny >= 0 && ny < rows && !visited.Contains(new Vector3Int(nx, ny, 0)))
                {
                    // Kiểm tra xem ô kề có chứa Pokemon không
                    GameObject pokemonAtNeighbor = GetPokemonAtPosition(new Vector3Int(nx, ny, 0));
                    if (pokemonAtNeighbor == null || pokemonAtNeighbor == pokemon1 || pokemonAtNeighbor == pokemon2)
                    {
                        Debug.Log("AAAAAAAAAAAAa");
                        // Nếu ô kề không chứa Pokemon hoặc là Pokemon 1 hoặc là Pokemon 2, tiếp tục tìm kiếm
                        queue.Enqueue(new Vector3Int(nx, ny, 0));
                        visited.Add(new Vector3Int(nx, ny, 0));
                    }
                }
            }
        }

        // Nếu không tìm thấy đường đi, trả về false
        return false;
    }


    private GameObject GetPokemonAtPosition(Vector3Int position)
    {
        foreach (GameObject pikachu in pikachus)
        {
            if (pikachu != null) // Thêm kiểm tra null vào đây
            {
                Vector3Int pikachuPos = GetGridPosition(pikachu.transform.position);
                if (pikachuPos == position)
                {
                    return pikachu;
                }
            }
        }
        return null;
    }
    private Vector3Int GetGridPosition(Vector3 position)
    {
        // Chuyển đổi vị trí thế giới thành vị trí trong lưới
        int x = Mathf.RoundToInt((position.x - pikachuContainer.transform.position.x + (columns - 1) * x_spacing / 2) / x_spacing);
        int y = Mathf.RoundToInt((pikachuContainer.transform.position.y - position.y + (rows - 1) * y_spacing / 2) / y_spacing);

        return new Vector3Int(x, y, 0);
    }
}
