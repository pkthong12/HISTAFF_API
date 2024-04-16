/*
using API.All.DbContexts;
using API.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using RegisterServicesWithReflection.Services.Base;
using System.Diagnostics;

namespace API.All.Services
{
    [TransientRegistration]
    public class MigrationRepository : IMigrationRepository
    {

        private readonly PatternDbContext _oldContext;
        private readonly FullDbContext _newContext;
        private readonly AppSettings _appSettings;

        public MigrationRepository(PatternDbContext oldContext, FullDbContext newContext, IOptions<AppSettings> options)
        {
            _oldContext = oldContext;
            _newContext = newContext;
            _appSettings = options.Value;
        }

        public void SysGroupAndSysUserMigrate()
        {

            _newContext.Database.BeginTransaction();
            try
            {
                // Randomly select 5 articles and their paragraphs and tags from the old site
                // Add the result to the new site

                // select migrated oldIds
                var migratedOldIds = _newContext.SysMigrationLogs.AsNoTracking().Where(x => x.TABLE_NAME == "SYS_GROUP").Select(x => x.OLD_ID).ToList();

                // Select randomly 5 groups which have not been migrated
                DateTime gateDate = DateTime.UtcNow;
                var newFive = _oldContext.SysGroups.Where(x => migratedOldIds == null || migratedOldIds.All(id => id != x.ID)).OrderBy(x => Guid.NewGuid()).Take(5).ToList();

                List<SysGroupDTO> oldGroupDTOs = [];
                newFive.ForEach(a =>
                {

                    var group = new SysGroupDTO()
                    {
                        Id = a.ID,
                        Name = a.NAME,
                        Note = a.NOTE,
                        Code = a.CODE,
                        IsAdmin = a.IS_ADMIN,
                        IsActive = a.IS_ACTIVE,
                        IsSystem = a.IS_SYSTEM
                    };

                    List<SysUserDTO> userOldDTOs = [];
                    var oldUsers = _oldContext.SysUsers.Where(x => x.GROUP_ID == group.Id).ToList();

                    userOldDTOs?.ForEach(u =>
                                    {
                                        userOldDTOs.Add(new SysUserDTO()
                                        {
                                            Id = u.Id,
                                            Discriminator,
                                            GroupId,
                                            Fullname,
                                            IsAdmin,
                                            IsRoot,
                                            Avatar,
                                            CreatedDate,
                                            UpdatedDate
                                            Username,
                                            Normalizedusername,
                                            Email,
                                            Normalizedemail,
                                            Emailconfirmed,
                                            IsLock,
                                            Passwordhash,
                                            Securitystamp,
                                            Concurrencystamp,
                                            Phonenumber,
                                            Phonenumberconfirmed,
                                            Twofactorenabled,
                                            Lockoutend,
                                            Lockoutenabled,
                                            Accessfailedcount,
                                            IsFirstLogin,
                                            IsPortal,
                                            IsWebapp,
                                            EmployeeId,


                                        });
                                    });
                    article.Paragraphs = paragraphOldDTOs;

                    List<TagOldDTO> tagOldDTOs = [];
                    var oldTags = (from at in _oldContext.ArtTAs.Where(x => x.ArtID == article.Id)
                                   from t in _oldContext.TAs.Where(x => x.TAID == at.TAID).DefaultIfEmpty()
                                   select new
                                   {
                                       Id = at.TAID,
                                       Tag = t.TA
                                   }).Distinct().ToList();


                    oldTags?.ForEach(t =>
                                    {
                                        tagOldDTOs.Add(new TagOldDTO()
                                        {
                                            Id = t.Id,
                                            Tag = t.Tag
                                        });
                                    });
                    article.Tags = tagOldDTOs;

                    articleOldDTOs.Add(article);
                });


                articleOldDTOs.ForEach(a =>
                {
                    var oldId = a.Id;
                    var migrationStart = DateTime.UtcNow;

                    var newArticle = new ART_ARTICLE()
                    {
                        CATEGORY_ID = a.CategoryId,
                        CAPTION = a.Caption,
                        IMAGE_URL = a.ImageUrl,
                        BODY = a.Body,
                        CREATED_DATE = a.CreatedDate,
                        UPDATED_DATE = a.UpdatedDate,
                        PUBLISHED_DATE = a.PublishedDate,
                        ADMIN_ONLY = a.AdminOnly,
                        LOGIN_REQUIRED = a.LoginRequired
                    };
                    _newContext.ArtArticles.Add(newArticle);
                    _newContext.SaveChanges();
                    var newId = newArticle.ID;

                    a.Paragraphs.ForEach(p =>
                    {
                        var newParagraph = new ART_PARAGRAPH()
                        {
                            ARTICLE_ID = newId,
                            BODY = p.ParagraphBody,
                            IMAGE_URL = p.ParagraphImageUrl,
                            QUEUE = p.ParagraphQueue,
                            IS_WAVESURFER = p.Wavesurfer,
                            WAVESURFER_URL = p.WavesurferPath,
                            WAVESURFER_TITLE = p.WavesurferTitle,
                            WAVESURFER_DESCRIPTION = p.WavesurferDescription,
                            IS_COMPONENT = p.IsComponent,
                            COMPONENT_NAME = p.ComponentName
                        };
                        _newContext.ArtParagraphs.Add(newParagraph);
                        _newContext.SaveChanges();
                    });

                    a.Tags?.ForEach(t =>
                    {

                        var tagTryFind = _newContext.ArtTags.SingleOrDefault(x => x.TAG_NAME == t.Tag);

                        long tagId;

                        if (tagTryFind == null)
                        {
                            var newTag = new ART_TAG()
                            {
                                TAG_NAME = t.Tag,
                            };
                            _newContext.ArtTags.Add(newTag);
                            _newContext.SaveChanges();
                            tagId = newTag.ID;

                        }
                        else
                        {
                            tagId = tagTryFind.ID;
                        }

                        var newArtArticleTag = new ART_ARTICLE_TAG()
                        {
                            ARTICLE_ID = newId,
                            TAG_ID = tagId
                        };

                        var artTagTryFind = _newContext.ArtArticleTags.SingleOrDefault(x => x.ARTICLE_ID == a.Id && x.TAG_ID == tagId);

                        if (artTagTryFind == null)
                        {
                            _newContext.ArtArticleTags.Add(newArtArticleTag);
                            _newContext.SaveChanges();
                        }

                    });

                    var migrationEnd = DateTime.Now;
                    var artMigrationLog = new ART_MIGRATION_LOG()
                    {
                        OLD_ID = oldId,
                        NEW_ID = newId,
                        CAPTION = a.Caption,
                        MIGRATION_START = migrationStart,
                        MIGRATION_END = migrationEnd,
                        PARAGRAPH_COUNT = (short)a.Paragraphs.Count,
                        TAG_COUNT = (short)(a.Tags == null ? 0 : a.Tags.Count),
                        CREATED_DATE = migrationEnd,
                    };
                    _newContext.ArtMigrationLogs.Add(artMigrationLog);
                    _newContext.SaveChanges();
                });

                _newContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                _newContext.Database.RollbackTransaction();
            }

        }

    }
}
*/