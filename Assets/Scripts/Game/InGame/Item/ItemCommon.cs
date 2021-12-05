
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
    //아이템 관련 명령처리할 때에 아이템의 복잡도가 올라가면 올라갈 수록 왜 실패했는지 찾기 힘들어 지므로 
    //이렇게 Exeption을 구현해 return해줍니다.
    public enum ItemExeption : int 
    {
        Succeeded = 0,
        //todo add failed exeption
        Failed = 1000,
        Failed_OverMaxAmount,       //아이템 수량 초과
        Failed_OverMinAmount,       //아이템 최소 수량 이하 (삭제 시 0아래로 떨어지는지)
        Failed_DiffrentItem,        //아이템이 다를경우 
        Failed_AlreadyEquipItem,    //아이템이 슬롯에 이미 장착되어 있는 경우
        Failed_WrongOwner,          //잘못된 오너에게 요청했을 경우
        Failed_NullOwner,           //오너가 Null일 경우
        Faield_AlreadyHasOwner,     //이미 오너가 있는 아이템일 경우(다른쪽에서도 해당 아이템을 참조하고 있을 가능성이 있으므로 먼저 오너십을 끊고
                                    //장착 시도를 해야한다)
        Failed_Casting,             //캐스팅(형변환) 실패
        Failed_ContainerIsFull,     //컨테이너 가득 참
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