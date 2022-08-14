namespace Ordering.Domain.Enums;

public enum EOrderStatus
{
    New = 1,//start with 1,0 is used for filter All = 0
    Pending, //ordering is pending, not any activities for a period time
    Paid, //order is paid
    Shipping, //order is on the shipping
    Fullfilled, //order is fullfilled
}