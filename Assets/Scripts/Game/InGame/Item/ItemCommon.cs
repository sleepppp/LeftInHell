
namespace Project
{
    using Project.GameData;

    public enum ItemType : int
    {
        All = 0,
        Healkit = 1,
        ItemBox = 2,
        Bag = 3
    }
    //������ ���� ���ó���� ���� �������� ���⵵�� �ö󰡸� �ö� ���� �� �����ߴ��� ã�� ����� ���Ƿ� 
    //�̷��� Exeption�� ������ return���ݴϴ�.
    public enum ItemExeption : int 
    {
        Succeeded = 0,
        //todo add failed exeption
        Failed = 1000,
        Failed_OverMaxAmount,       //������ ���� �ʰ�
        Failed_OverMinAmount,       //������ �ּ� ���� ���� (���� �� 0�Ʒ��� ����������)
        Failed_DiffrentItem,        //�������� �ٸ���� 
        Failed_AlreadyEquipItem,    //�������� ���Կ� �̹� �����Ǿ� �ִ� ���
        Failed_WrongOwner,          //�߸��� ���ʿ��� ��û���� ���
        Failed_NullOwner,           //���ʰ� Null�� ���
        Faield_AlreadyHasOwner,     //�̹� ���ʰ� �ִ� �������� ���(�ٸ��ʿ����� �ش� �������� �����ϰ� ���� ���ɼ��� �����Ƿ� ���� ���ʽ��� ����
                                    //���� �õ��� �ؾ��Ѵ�)
        Failed_Casting,             //ĳ����(����ȯ) ����
        Failed_ContainerIsFull,     //�����̳� ���� ��
    }

    public interface IGetItemData
    {
        ItemRecord GetItemRecord();
        ItemTypeRecord GetItemTypeRecord();
        int Amount { get; }
        int MaxAmount { get; }
        int Width { get; }
        int Height { get; }
        bool IsStackable { get; }
        bool IsConsumeable { get; }
    }

    public interface IHandleItemData
    {
        ItemExeption IsPossiblyAddAmount(int amount);
        ItemExeption IsPossiblyRemoveAmount(int amount);
        ItemExeption TryAddAmount(int amount);
        ItemExeption TryRemoveAmount(int amount);
    }
}