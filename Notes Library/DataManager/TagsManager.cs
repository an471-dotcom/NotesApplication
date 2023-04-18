using Notes_Library.Data.DataHandler;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Note;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using Notes_Library.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Security.Cryptography.Core;

namespace Notes_Library.DataManager
{
    public class TagsManager
    {
        private static TagsManager _instance = null;

        private TagsManager() { }
        public static TagsManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TagsManager();
                }
                return _instance;
            }
        }
        private ITagDBHandler _tagHandler = TagDBHandler.GetInstance;
       

        public async Task<List<Tag>> GetNoteTags(int noteId)
        {
            return await _tagHandler.GetNoteTags(noteId); 
        }

        public async void CreateNoteTag(string tagName,int userId,int noteId,string tagColor, IUsecaseCallback<CreateNoteTagResponse> callback)
        {
            try
            {
                Tag tag;
                tag =await _tagHandler.GetTagByName(tagName,userId);
                if(tag == null)
                {
                    tag = await _tagHandler.AddTag(new Tag { Name = tagName, UserId = userId ,TagColor = tagColor});
                }
                
                var duplicate = await CheckDuplicate(tag.Id, noteId);

                if (!duplicate)
                {
                    await _tagHandler.AddTagToNote(new TaggedNotes { NoteId = noteId, TagId = tag.Id });
                }
                else
                {
                    throw new Exception("Tag already addded");
                }
               
                callback?.OnSuccess(new CreateNoteTagResponse { Tag = tag  });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
           
        }

        public async void DeleteNoteTag(int noteId,int tagId,IUsecaseCallback<DeleteNoteTagResponse> callback)
        {
            try
            {
              
                int result = await _tagHandler.DeleteNoteTag(noteId, tagId);
                callback?.OnSuccess(new DeleteNoteTagResponse { TagId = tagId });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void GetNoteTagsSuggestion(string input,int userId,IUsecaseCallback<GetTagsSuggestionResponse> callback)
        {
            try
            {
                var tags = await _tagHandler.GetNoteTagSuggestion(input, userId);
                callback?.OnSuccess(new GetTagsSuggestionResponse { Tags= tags });
                    
            }
            catch(Exception e )
            {
                callback?.OnError(e);
            }
        }

        public async void UpdateTag(int tagId,string tagName,string tagColor, IUsecaseCallback<UpdateTagResponse> callback)
        {
            try
            {
                var tag = await _tagHandler.GetTag(tagId);
                tag.Name = tagName;
                tag.TagColor= tagColor;
                var updatedTag = await _tagHandler.UpdateTag(tag);
                callback?.OnSuccess(new UpdateTagResponse { UpdatedTag= updatedTag });
                TagNotificationcs.InvokeTagUpdated(updatedTag);
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async Task<bool> CheckDuplicate(int tagId,int noteId)
        {
            return await _tagHandler.IsTagAlreadyAdded(tagId, noteId);    
        }
        public async void GetAllTags(int userId,IUsecaseCallback<GetAllTagsResponse> callback)
        {
            try
            {
                var tags = await _tagHandler.GetUserTags(userId);
                callback?.OnSuccess(new GetAllTagsResponse { Tags = tags });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void DeleteTag(int tagId,IUsecaseCallback<DeleteTagResponse> callback)
        {
            try
            {
                await _tagHandler.DeleteTag(tagId);

                callback?.OnSuccess(new DeleteTagResponse { DeletedTagId= tagId });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }

        }
    }
}
