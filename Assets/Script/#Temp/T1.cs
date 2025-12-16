using UnityEngine;

public class T1 : MonoBehaviour
{
    void Start() {
        
        temp1();
    }

    void temp1() {
        string a = 700000.ToString("#,#.#");
        Debug.Log(a);
    }

    void temp2() {
        int[] f = { 1, 1, 0, 0, 0 };
        string Output = @"INSERT INTO `food` 
            	(`id_brand`, `id_food`, `name`, `price`, `quantity_per_set`, `describle`, `rate`, `limitted_quantity`, `image_path`) 
            VALUES 
            	('0000000001', '1', 'CupCake Dâu tây', '45000', 'combo 4 cái', 'cupcake phủ kem vanila, topping dâu tây + hạt chocolate. Bán theo set combo 4 cái mỗi set.', '4.6', NULL, NULL)
            	('0000000002', '1', 'Bánh kem Chocolate', '70000', 'combo 6 cái', 'bánh kem 6 lớp, phủ socola. Ưu đãi ngày lễ tình yêu. Bán theo set, 6 cái mỗi set.', '4.8', '20', NULL)";
        for (int i = 3; i <= 40; i++) {
            int id_Brand_Int = (i % 5 == 0) ? 5 : (i % 5);
            string id_brand = id_Brand_Int.ToString("D10");
            int id_Food_Int = ++f[id_Brand_Int - 1];
            string id_food = id_Food_Int.ToString();
            string name = $"food #{i}";
            string price = (Random.Range(1, 1000) * 1000).ToString();
            string quantity_per_set = "combo " + Random.Range(1, 10).ToString() + " cái";
            string describle = "Mô tả cho món ăn " + name;
            string rate = (Random.Range(30, 50) / 10.0f).ToString("F1");
            string limitted_quantity = (Random.Range(0, 2) == 0) ? Random.Range(10, 100).ToString() : "NULL";
            string image_path = "NULL";

            Output += $",\n\t('{id_brand}', '{id_food}', '{name}', '{price}', '{quantity_per_set}', '{describle}', '{rate}', {limitted_quantity}, {image_path})";
            
            Debug.Log(Output);
        }
    }
}
