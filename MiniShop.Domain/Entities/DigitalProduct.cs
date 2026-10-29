
namespace MiniShop.Domain.Entities;

public class DigitalProduct : Product
{
    public int LicenseDurationDays { get; private set; }

    public DigitalProduct(int id,string name,decimal price,int licenseDurationDays) : base(id, name, price)
    {
        if (licenseDurationDays <= 0)
            throw new ArgumentException("Срок лицензии должен быть больше нуля.");

        LicenseDurationDays = licenseDurationDays;
    }

    public override decimal GetCalculatedFinalPrice()
    {
        return Price;
    }


    public void ChangeLicenseDurationDays(int newDurationDays)
    {
        if (newDurationDays <= 0)
            throw new ArgumentException("Срок лицензии должен быть больше нуля.");

        LicenseDurationDays = newDurationDays;
    }


    public override string ToString()
    {
        return base.ToString() + $" | Лицензия: {LicenseDurationDays} дней";
    }
}
