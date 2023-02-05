using BlazorApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;


namespace BlazorApp.MyCode
{
    public class ControlDB
    {
        public readonly string FullPathDB = "./Data/db.json";

        private readonly TVs tvs = new TVs();

        public TVs TVs
        {
            get
            {
                if (tvs.ListTVs.Count > 0) return tvs;
                else return null;
            }
        }


        private void InitTVs()
        {
            var tv = new TV()
            {
                Brand = "Рубин",
                Model = "Рубин-567В",
                Diagonal = 52,
                ProductionYear = 2000
            };

            tvs.ListTVs.Clear();
            tvs.ListTVs.Add(tv);
        }



        public async Task<TVs> ReadFromDB()
        {
            // Запускаем операцию чтения базы данных в отдельном потоке
            return await Task.Run(async () =>
            {
                if (FullPathDB != null)
                {

                    var streamReader = new StreamReader(FullPathDB);

                    try
                    {
                        tvs.ListTVs = JsonSerializer.Deserialize<TVs>(streamReader.ReadToEnd()).ListTVs;
                    }
                    catch
                    {
                        // Если что-то непредвиденное заново инициализируем данные.
                        InitTVs();

                        await WriteToDB();
                    }

                    streamReader.Close();
                }

                return tvs;
            });
        }



        public async Task WriteToDB()
        {
            await Task.Run(() =>
            {
                var serializerOptions = new JsonSerializerOptions
                {
                    // Формирует вид, привлекательный для чтения и печати.
                    WriteIndented = true,

                    // Настройка кодировки символов для кирилицы.
                    // По умолчанию сериализатор выполняет escape - последовательность символов,
                    // отличных от ASCII.То есть он заменяет их \uxxxx,
                    // где xxxx является кодом Юникода символа.
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic)
                };


                string s = JsonSerializer.Serialize<TVs>(tvs, serializerOptions);
                var sw = new StreamWriter(FullPathDB);
                sw.Write(s);
                sw.Close();
            });

        }



        public async Task AddTV()
        {
            await Task.Run(async () =>
            {
                var tv = new TV()
                {
                    Brand = "Марка телевизора"
                };

                tvs.ListTVs.Add(tv);

                await WriteToDB();
            });
        }



        public async Task RemoveTV(TV tv)
        {
            await Task.Run(async () =>
            {
                if (tv != null)
                {
                    tvs.ListTVs.Remove(tv);

                    await WriteToDB();
                }
            });
        }
    }
}