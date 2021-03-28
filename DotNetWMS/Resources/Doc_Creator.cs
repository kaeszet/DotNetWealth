using DotNetWMS.Data;
using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public class Doc_Creator
    {
        public async Task GenerateAndSaveDocument(Item item, DotNetWMSContext _context, string userName, string method, string from, string to)
        {
            Doc_Assignment document = GenerateDocument(item, _context, userName, method, from, to);

            if (document != null)
            {
                _context.Add(document);
                AddToInfobox(document, _context, userName);

                await _context.SaveChangesAsync();
            }
        }

        public async Task GenerateAndSaveDocument(ItemAssignmentConfirmationViewModel model, DotNetWMSContext _context, string userName, Doc_Titles docTitle)
        {
            Doc_Assignment document = GenerateDocument(model, _context, userName, docTitle);

            if (document != null)
            {
                _context.Add(document);
                AddToInfobox(document, _context, userName);

                await _context.SaveChangesAsync();
            }
        }

        private Doc_Assignment GenerateDocument(Item item, DotNetWMSContext _context, string userName, string method, string from, string to)
        {
            DateTime currentDate = DateTime.Now;
            Doc_Titles docTitle = DocumentTitleGenerator(method, to);
            int docCount = _context.Doc_Assignments.Count() + 1;
            var loggedUser = _context.Users.FirstOrDefault(u => u.NormalizedUserName == userName);

            var items = _context.Items.Where(i => i.Id == item.Id).ToList();

            switch (method)
            {
                case "Assign_to_employee":

                    return new Doc_Assignment
                    {
                        DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
                        Title = docTitle.ToString(),
                        CreationDate = currentDate,
                        UserFrom = !string.IsNullOrEmpty(from) ? from : loggedUser.Id,
                        UserFromName = !string.IsNullOrEmpty(from) ? _context.Users.Find(from)?.FullNameForDocumentation : loggedUser.FullNameForDocumentation,
                        UserTo = !string.IsNullOrEmpty(to) ? to : loggedUser.Id,
                        UserToName = !string.IsNullOrEmpty(to) ? _context.Users.Find(to)?.FullNameForDocumentation : loggedUser.FullNameForDocumentation,
                        Items = items

                    };

                case "Assign_to_warehouse":

                    int? WarehouseFromId = null;
                    int? WarehouseToId = null;

                    try
                    {
                        if (!string.IsNullOrEmpty(from))
                        {
                            WarehouseFromId = Convert.ToInt32(from);
                        }
                        
                    }
                    catch (Exception)
                    {
                        WarehouseFromId = null;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(to))
                        {
                            WarehouseToId = Convert.ToInt32(to);
                        }
                        
                    }
                    catch (Exception)
                    {
                        WarehouseToId = null;
                    }

                    return new Doc_Assignment
                    {
                        DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
                        Title = docTitle.ToString(),
                        CreationDate = currentDate,
                        UserFrom = WarehouseFromId == null ? loggedUser.Id : null,
                        UserFromName = WarehouseFromId == null ? loggedUser.FullNameForDocumentation : null,
                        UserTo = WarehouseToId == null ? loggedUser.Id : null,
                        UserToName = WarehouseToId == null ? loggedUser.FullNameForDocumentation : null,
                        WarehouseFrom = WarehouseFromId,
                        WarehouseFromName = _context.Warehouses.Find(WarehouseFromId)?.FullNameForDocumentation,
                        WarehouseTo = WarehouseToId,
                        WarehouseToName = _context.Warehouses.Find(WarehouseToId)?.FullNameForDocumentation,
                        Items = items

                    };
                    
                case "Assign_to_external":

                    int? ExternalFromId = null;
                    int? ExternalToId = null;

                    try
                    {
                        if (!string.IsNullOrEmpty(from))
                        {
                            ExternalFromId = Convert.ToInt32(from);
                        }

                    }
                    catch (Exception)
                    {
                        ExternalFromId = null;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(to))
                        {
                            ExternalToId = Convert.ToInt32(to);
                        }

                    }
                    catch (Exception)
                    {
                        ExternalToId = null;
                    }

                    return new Doc_Assignment
                    {
                        DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
                        Title = docTitle.ToString(),
                        CreationDate = currentDate,
                        UserFrom = ExternalFromId == null ? loggedUser.Id : null,
                        UserFromName = ExternalFromId == null ? loggedUser.FullNameForDocumentation : null,
                        UserTo = ExternalToId == null ? loggedUser.Id : null,
                        UserToName = ExternalToId == null ? loggedUser.FullNameForDocumentation : null,
                        ExternalFrom = ExternalFromId,
                        ExternalFromName = _context.Externals.Find(ExternalFromId)?.FullNameForDocumentation,
                        ExternalTo = ExternalToId,
                        ExternalToName = _context.Externals.Find(ExternalToId)?.FullNameForDocumentation,
                        Items = items

                    };
                    
                default:
                    return null;
            }


            //if (docTitle == Doc_Titles.ZW || docTitle == Doc_Titles.RW || docTitle == Doc_Titles.PZ)
            //{
            //    doc = new Doc_Assignment
            //    {
            //        DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
            //        Title = docTitle.ToString(),
            //        CreationDate = currentDate,
            //        UserFrom = !string.IsNullOrEmpty(from) ? from : null,
            //        UserFromName = !string.IsNullOrEmpty(from) ? _context.Users.Find(from)?.FullNameForDocumentation : null,
            //        UserTo = !string.IsNullOrEmpty(loggedUser.Id) ? loggedUser.Id : null,
            //        UserToName = !string.IsNullOrEmpty(loggedUser.FullNameForDocumentation) ? loggedUser.FullNameForDocumentation : null,
            //        WarehouseFrom = model.WarehouseId != null ? model.WarehouseId : null,
            //        WarehouseFromName = model.WarehouseId != null ? _context.Warehouses.Find(model.WarehouseId).FullNameForDocumentation : null,
            //        ExternalFrom = model.ExternalId != null ? model.ExternalId : null,
            //        ExternalFromName = model.ExternalId != null ? _context.Externals.Find(model.ExternalId).FullNameForDocumentation : null,
            //        Items = items

            //    };
            //}
            //else
            //{
            //    doc = new Doc_Assignment
            //    {
            //        DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
            //        Title = docTitle.ToString(),
            //        CreationDate = currentDate,
            //        UserFrom = !string.IsNullOrEmpty(loggedUser.Id) ? loggedUser.Id : null,
            //        UserFromName = !string.IsNullOrEmpty(loggedUser.FullNameForDocumentation) ? loggedUser.FullNameForDocumentation : null,
            //        UserTo = !string.IsNullOrEmpty(model.UserId) ? model.UserId : null,
            //        UserToName = !string.IsNullOrEmpty(model.UserId) ? _context.Users.Find(model.UserId).FullNameForDocumentation : null,
            //        WarehouseTo = model.WarehouseId != null ? model.WarehouseId : null,
            //        WarehouseToName = model.WarehouseId != null ? _context.Warehouses.Find(model.WarehouseId).FullNameForDocumentation : null,
            //        ExternalTo = model.ExternalId != null ? model.ExternalId : null,
            //        ExternalToName = model.ExternalId != null ? _context.Externals.Find(model.ExternalId).FullNameForDocumentation : null,
            //        Items = items

            //    };
            //}

        }

        private Doc_Assignment GenerateDocument(ItemAssignmentConfirmationViewModel model, DotNetWMSContext _context, string userName, Doc_Titles docTitle)
        {
            List<int> itemIds = new List<int>();

            DateTime currentDate = DateTime.Now;
            //Doc_Titles title = DocumentTitleGenerator(model);
            int docCount = _context.Doc_Assignments.Count() + 1;
            var loggedUser = _context.Users.FirstOrDefault(u => u.NormalizedUserName == userName);

            foreach (var item in model.Items)
            {
                itemIds.Add(item.Id);
            }

            var items = _context.Items.Where(i => itemIds.Contains(i.Id)).ToList();

            Doc_Assignment doc;

            if (docTitle == Doc_Titles.ZW || docTitle == Doc_Titles.RW || docTitle == Doc_Titles.PZ)
            {
                doc = new Doc_Assignment
                {
                    DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
                    Title = docTitle.ToString(),
                    CreationDate = currentDate,
                    UserFrom = !string.IsNullOrEmpty(model.UserId) ? model.UserId : null,
                    UserFromName = !string.IsNullOrEmpty(model.UserId) ? _context.Users.Find(model.UserId).FullNameForDocumentation : null,
                    UserTo = !string.IsNullOrEmpty(loggedUser.Id) ? loggedUser.Id : null,
                    UserToName = !string.IsNullOrEmpty(loggedUser.FullNameForDocumentation) ? loggedUser.FullNameForDocumentation : null,
                    WarehouseFrom = model.WarehouseId != null ? model.WarehouseId : null,
                    WarehouseFromName = model.WarehouseId != null ? _context.Warehouses.Find(model.WarehouseId).FullNameForDocumentation : null,
                    ExternalFrom = model.ExternalId != null ? model.ExternalId : null,
                    ExternalFromName = model.ExternalId != null ? _context.Externals.Find(model.ExternalId).FullNameForDocumentation : null,
                    Items = items

                };
            }
            else
            {
                doc = new Doc_Assignment
                {
                    DocumentId = DocumentIdGenerator(currentDate, docTitle, docCount),
                    Title = docTitle.ToString(),
                    CreationDate = currentDate,
                    UserFrom = !string.IsNullOrEmpty(loggedUser.Id) ? loggedUser.Id : null,
                    UserFromName = !string.IsNullOrEmpty(loggedUser.FullNameForDocumentation) ? loggedUser.FullNameForDocumentation : null,
                    UserTo = !string.IsNullOrEmpty(model.UserId) ? model.UserId : null,
                    UserToName = !string.IsNullOrEmpty(model.UserId) ? _context.Users.Find(model.UserId).FullNameForDocumentation : null,
                    WarehouseTo = model.WarehouseId != null ? model.WarehouseId : null,
                    WarehouseToName = model.WarehouseId != null ? _context.Warehouses.Find(model.WarehouseId).FullNameForDocumentation : null,
                    ExternalTo = model.ExternalId != null ? model.ExternalId : null,
                    ExternalToName = model.ExternalId != null ? _context.Externals.Find(model.ExternalId).FullNameForDocumentation : null,
                    Items = items

                };
            }

            

            return doc;
        }

        //private async Task SaveDocument(DotNetWMSContext _context, Doc_Assignment doc)
        //{
        //    _context.Add(doc);
        //    await _context.SaveChangesAsync();
        //}

        private string DocumentIdGenerator(DateTime time, Doc_Titles title, int docsInContext)
        {
            string docCount = docsInContext.ToString();
            StringBuilder sb = new StringBuilder();
            string divider = "/";

            sb.Append(title);
            sb.Append(divider);
            sb.Append(time.Year);
            sb.Append(divider);
            sb.Append(time.Month);
            sb.Append(divider);
            sb.Append(time.Day);
            sb.Append(divider);

            switch (docCount.Length)
            {
                case 1:
                    sb.Append("0000");
                    sb.Append(docCount);
                    break;
                case 2:
                    sb.Append("000");
                    sb.Append(docCount);
                    break;
                case 3:
                    sb.Append("00");
                    sb.Append(docCount);
                    break;
                case 4:
                    sb.Append("0");
                    sb.Append(docCount);
                    break;
                default:
                    sb.Append(docCount);
                    break;
            }

            sb.Append(divider);
            sb.Append(MillisOfDay(time));

            return sb.ToString();
        }
        /// <summary>
        /// Method with logic for checking correct document title which is necessary to generate documentId
        /// </summary>
        /// <param name = "from" > ID of User, Warehouse or External FROM whom the item is transferred</param>
        /// <param name = "to" > ID of User, Warehouse or External TO whom the item is transferred</param>
        /// <returns>Returns correct title from enum two dimensional array</returns>
        private Doc_Titles DocumentTitleGenerator(int? from, int? to)
        {

            Doc_Titles[,] docTitlesArray = new Doc_Titles[,]
            {
                { Doc_Titles.P, Doc_Titles.PW, Doc_Titles.PW, Doc_Titles.WZ },
                { Doc_Titles.ZW, Doc_Titles.PW, Doc_Titles.ZW, Doc_Titles.WZ },
                { Doc_Titles.ZW, Doc_Titles.RW, Doc_Titles.MM, Doc_Titles.WZ },
                { Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.WZ },
            };

            return docTitlesArray[(int)from + 1, (int)to + 1];
        }

        private Doc_Titles DocumentTitleGenerator(ItemAssignmentConfirmationViewModel model)
        {
            if (!string.IsNullOrEmpty(model.UserId) || model.WarehouseId != null) return Doc_Titles.PW;
            else return model.ExternalId != null ? Doc_Titles.WZ : Doc_Titles.ZW;

        }

        private Doc_Titles DocumentTitleGenerator(string method, string to)
        {
            return method switch
            {
                "Assign_to_employee" => !string.IsNullOrEmpty(to) ? Doc_Titles.PW : Doc_Titles.ZW,
                "Assign_to_warehouse" => !string.IsNullOrEmpty(to) ? Doc_Titles.MM : Doc_Titles.RW,
                "Assign_to_external" => !string.IsNullOrEmpty(to) ? Doc_Titles.WZ : Doc_Titles.PZ,
                _ => Doc_Titles.P,
            };
        }


        /// <summary>
        /// Method for calculating millisecond after today 0:00:00
        /// </summary>
        /// <param name = "date" > Time of generating the document</param>
        /// <returns>Amount of milliseconds</returns>
        private long MillisOfDay(DateTime date)
        {
            DateTime today = new DateTime(date.Year, date.Month, date.Day);
            return (long)(DateTime.Now - today).TotalMilliseconds;
        }

        private void AddToInfobox(Doc_Assignment doc, DotNetWMSContext _context, string userName)
        {
            Infobox info = new Infobox();
            //string UserIdentityName = !string.IsNullOrEmpty(User?.Identity?.Name) ? User.Identity.Name : "";
            string loggedUserId = _context.Users.FirstOrDefault(u => u.NormalizedUserName == userName)?.Id;

            if (doc.UserTo != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == doc.UserTo);

                if (user != null)
                {
                    info.Title = $"Otrzymałeś dokument do potwierdzenia od użytkownika {userName}";
                    info.Message = $"Wygenerowano dokument nr {doc.DocumentId}, który wymaga potwierdzenia. Aby to zrobić, kliknij \"Potwierdź\"";
                    info.ReceivedDate = doc.CreationDate;
                    info.UserId = string.IsNullOrEmpty(user.Id) ? loggedUserId : user.Id;
                    info.DocumentId = doc.DocumentId;

                    _context.Infoboxes.Add(info);
                }
            }
            else
            {
                info.Title = "Potwierdź wygenerowany przez siebie dokument";
                info.Message = $"Wygenerowano dokument nr {doc.DocumentId}, który wymaga potwierdzenia. Aby to zrobić, kliknij \"Potwierdź\"";
                info.ReceivedDate = doc.CreationDate;
                info.UserId = loggedUserId;
                info.DocumentId = doc.DocumentId;

                _context.Infoboxes.Add(info);
            }
        }
    }
}
