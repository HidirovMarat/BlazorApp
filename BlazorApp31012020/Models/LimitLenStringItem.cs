using BlazorApp.MyCode;


namespace BlazorApp.Models
{
    // Класс-родитель для ограничения длины строковых переменных в базе данных.
    // Если новую таблицу добавить в базу данных можно 
    // наследовать этот класс для упрощения кода.
    public class LimitLenStringItem
    {
        private string brand;
        public string Brand
        {
            get { return brand; }
            set
            {
                if (value != null && value.Length < Constants.MaxLengthString)
                {
                    brand = value;
                }
            }
        }


        private string model;
        public string Model
        {
            get { return model; }
            set
            {
                if (value != null && value.Length < Constants.MaxLengthString)
                {
                    model = value;
                }
            }
        }
    }
}
