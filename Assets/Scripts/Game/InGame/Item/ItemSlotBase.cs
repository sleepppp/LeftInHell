
namespace Project
{
    //�������� �����ϴ� ���� ���´� ũ��
    //1. ��������
    //2. �κ��丮 ����
    //�� ������ ������
    //���
    //1. ������ ���� �� ����
    public interface IItemSlot
    {
        bool IsEmpty();
        ItemExeption IsPossiblyEquipItem(InventoryItem inventoryItem);
        ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem);
        ItemExeption TryEquipItem(InventoryItem inventoryItem);
        ItemExeption TryDisarmItem(InventoryItem inventoryItem);
        IItemContainer Owner { get; }
        InventoryItem InventoryItem { get; }
    }
}
