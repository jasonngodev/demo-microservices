namespace Shared.DTOs.Inventory;

public class PurchaseProductDto
{
    public string ItemNo { get; set; }
    
    public string DocumentNo { get; set; }//PO-2022-07-xxxx
    
    public string ExternalDocumentNo { get; set; }
    
    public  int Quantity { get; set; }
}