using NUnit.Framework.Api;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class T1 : MonoBehaviour
{
    public TMP_Dropdown dd;

    void Start() {
        //WorkWithServer.Instance.Get_SlotList_OfStore(1, 1, null, null);
        //WorkWithServer.Instance.Get_SlotZONElist_OfStore(1, 1, null, null);
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

    public void temp3() {
        Debug.Log(dd.value);
    }

    public void temp4() {
        if (Static_Info.UserInfo is null) Debug.Log("NoStaticUser");
        Debug.Log(Static_Info.UserInfo.username);
    }

    public void temp5() {
        string query = "INSERT INTO `table_slot` " +
            "\r\n\t(`id_brand`, `id_store`, `id_slot`, `name`, `capacity`, `price`, `zone`, `state`) " +
            "\r\nVALUES " +
            "\r\n\t('0000000001', '0000000001', '1', 'Bàn số 1', '4', '100000', 'Tầng 1', '0')," +
            "\r\n\t('0000000001', '0000000001', '2', 'Bàn số 2', '6', '120000', 'Tầng 1', '0')," +
            "\r\n\t('0000000001', '0000000001', '3', 'Bàn lớn', '10', '200000', 'Tầng 1', '0')," +
            "\r\n\t('0000000001', '0000000001', '4', 'Bàn số 3', '6', '120000', 'Tầng 2', '0')," +
            "\r\n\t('0000000001', '0000000001', '5', 'Bàn số 4', '4', '100000', 'Tầng 2', '0')," +
            "\r\n\t('0000000001', '0000000001', '6', 'Bàn ngoài trời', '8', '240000', 'Tầng 3', '0')";
        for (int i = 1; i <= 5; i++) {
            for (int j = 1; j <= 2; j++) {
                if (i == 1 && j == 1) continue;

                int banso = 0;
                int soLuongBan = Random.Range(5, 10);
                int tangso = 1;

                int z = 0;
                while (z < soLuongBan) {
                    z++;
                    nameChoise tableType = (nameChoise)Random.Range(0, 3);
                    string name = Patse(tableType, ref banso);
                    int capacity = (tableType == nameChoise.BanSo) ? Random.Range(1, 3)*2 : Random.Range(1, 10);
                    int price = capacity * Random.Range(1, 30) * 1000;
                    string zone = (Random.Range(0, 6) == 0) ? $"Tầng {tangso++}" : $"Tầng {tangso}";

                    query += $",\r\n\t('{i}', '{j}', '{z}', '{name}', '{capacity}', '{price}', '{zone}', '0')";
                }
            }
        }
        Debug.Log(query);

        string Patse(nameChoise id, ref int banso) {
            switch (id) {
                case nameChoise.BanSo:
                    return $"Bàn số {++banso}";
                case nameChoise.BanLon:
                    return "Bàn lớn";
                case nameChoise.BanNgoaiTroi:
                    return "Bàn ngoài trời";
                default:
                    return "Bàn chung";
            }
        }
    }

    private enum nameChoise {
        BanSo,
        BanLon,
        BanNgoaiTroi
    }

}
