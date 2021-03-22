using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public class Doc_Creator
    {
        public void GenerateAndSaveDocument(ItemAssignmentConfirmationViewModel model)
        {

        }

        //private void GenerateDocument(ItemAssignmentConfirmationViewModel model)
        //{
        //    DateTime currentDate = DateTime.Now;

        //    List<string> itemIds = new List<string>();
        //    Doc_Titles title = DocumentTitleGenerator(model);


        //    if (viewModel != null)
        //    {
        //        foreach (var item in viewModel)
        //        {
        //            if (item.AddToDocument) itemIds.Add(item.Id.ToString());
        //        }
        //    }

        //    var items = _context.Items.Where(i => itemIds.Contains(i.Id.ToString())).ToList();

        //    Doc_Assignment doc = new Doc_Assignment
        //    {
        //        DocumentId = DocumentIdGenerator(currentDate, title),
        //        Title = title.ToString(),
        //        CreationDate = currentDate,
        //        UserFrom = viewModel[0].FromIndex == 0 ? viewModel[0].From : "",
        //        UserFromName = viewModel[0].FromIndex == 0 ? _context.Users.Find(viewModel[0].From).FullNameForDocumentation : "",
        //        UserTo = viewModel[0].ToIndex == 0 ? viewModel[0].To : "",
        //        UserToName = viewModel[0].ToIndex == 0 ? _context.Users.Find(viewModel[0].To).FullNameForDocumentation : "",
        //        WarehouseFrom = viewModel[0].FromIndex == 1 ? Convert.ToInt32(viewModel[0].From) : 0,
        //        WarehouseFromName = viewModel[0].FromIndex == 1 ? _context.Warehouses.Find(Convert.ToInt32(viewModel[0].From)).FullNameForDocumentation : "",
        //        WarehouseTo = viewModel[0].ToIndex == 1 ? Convert.ToInt32(viewModel[0].To) : 0,
        //        WarehouseToName = viewModel[0].ToIndex == 1 ? _context.Warehouses.Find(Convert.ToInt32(viewModel[0].To)).FullNameForDocumentation : "",
        //        ExternalFrom = viewModel[0].FromIndex == 2 ? Convert.ToInt32(viewModel[0].From) : 0,
        //        ExternalFromName = viewModel[0].FromIndex == 2 ? _context.Externals.Find(Convert.ToInt32(viewModel[0].From)).FullNameForDocumentation : "",
        //        ExternalTo = viewModel[0].ToIndex == 2 ? Convert.ToInt32(viewModel[0].To) : 0,
        //        ExternalToName = viewModel[0].ToIndex == 2 ? _context.Externals.Find(Convert.ToInt32(viewModel[0].To)).FullNameForDocumentation : "",
        //        Items = items
        //    };
        //}

        //private void SaveDocument()
        //{
        //    AddToInfobox(doc);
        //    _context.Add(doc);
        //    await _context.SaveChangesAsync();
        //}

        //private string DocumentIdGenerator(DateTime time, Doc_Titles title)
        //{
        //    string docCount = (_context.Doc_Assignments.Count() + 1).ToString();
        //    StringBuilder sb = new StringBuilder();
        //    string divider = "/";

        //    sb.Append(title);
        //    sb.Append(divider);
        //    sb.Append(time.Year);
        //    sb.Append(divider);
        //    sb.Append(time.Month);
        //    sb.Append(divider);
        //    sb.Append(time.Day);
        //    sb.Append(divider);

        //    switch (docCount.Length)
        //    {
        //        case 1:
        //            sb.Append("0000");
        //            sb.Append(docCount);
        //            break;
        //        case 2:
        //            sb.Append("000");
        //            sb.Append(docCount);
        //            break;
        //        case 3:
        //            sb.Append("00");
        //            sb.Append(docCount);
        //            break;
        //        case 4:
        //            sb.Append("0");
        //            sb.Append(docCount);
        //            break;
        //        default:
        //            sb.Append(docCount);
        //            break;
        //    }

        //    sb.Append(divider);
        //    sb.Append(MillisOfDay(time));

        //    return sb.ToString();
        //}
        ///// <summary>
        ///// Method with logic for checking correct document title which is necessary to generate documentId
        ///// </summary>
        ///// <param name="from">ID of User, Warehouse or External FROM whom the item is transferred</param>
        ///// <param name="to">ID of User, Warehouse or External TO whom the item is transferred</param>
        ///// <returns>Returns correct title from enum two dimensional array</returns>
        //private Doc_Titles DocumentTitleGenerator(int? from, int? to)
        //{

        //    Doc_Titles[,] docTitlesArray = new Doc_Titles[,]
        //    {
        //        { Doc_Titles.P, Doc_Titles.PW, Doc_Titles.PW, Doc_Titles.WZ },
        //        { Doc_Titles.ZW, Doc_Titles.PW, Doc_Titles.ZW, Doc_Titles.WZ },
        //        { Doc_Titles.ZW, Doc_Titles.RW, Doc_Titles.MM, Doc_Titles.WZ },
        //        { Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.WZ },
        //    };

        //    return docTitlesArray[(int)from + 1, (int)to + 1];
        //}

        //private Doc_Titles DocumentTitleGenerator(ItemAssignmentConfirmationViewModel model)
        //{
        //    if (string.IsNullOrEmpty(model.UserId) || model.WarehouseId != null) return Doc_Titles.PW;
        //    else return model.ExternalId != null ? Doc_Titles.WZ : Doc_Titles.ZW;
            
        //}


        ///// <summary>
        ///// Method for calculating millisecond after today 0:00:00
        ///// </summary>
        ///// <param name="date">Time of generating the document</param>
        ///// <returns>Amount of milliseconds</returns>
        //private long MillisOfDay(DateTime date)
        //{
        //    DateTime today = new DateTime(date.Year, date.Month, date.Day);
        //    return (long)(DateTime.Now - today).TotalMilliseconds;
        //}
    }
}
